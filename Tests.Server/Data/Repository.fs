module Tests.Server.Data.Repository

open System.Threading.Tasks
open Server.Data
open Expecto
open Foq
open Shared.Dtos
open FakeItEasy

type EmptyContext<'a>() =
    interface IContext<'a> with
        member this.All () =
            Seq.empty<'a> |> Task.FromResult

        member this.Update _ =
            Task.FromResult(0)
            
module Stub = 
    let blogContext = EmptyContext<BlogEntry>()            
    let songContext = EmptyContext<Song>()
    
    

let blogRepoTests =
    testList
        "Blog Repository"
        [

            testCase "GetAll: returns all entries in context."
            <| fun _ ->
                // arrange
                let entries =
                    [
                        BlogEntry.create "one"
                        BlogEntry.create "two"
                        BlogEntry.create "three"
                    ]
            
                let context =
                    Mock<IContext<BlogEntry>>()
                        .Setup(fun r -> <@ r.All() @>)
                        .Returns(entries |> Seq.ofList |> Task.FromResult)
                        .Create()
            
                let target = BlogRepository(context) :> IRepository<BlogEntry>
            
                // act
                let result =
                    target.GetAll()
                    |> Async.RunSynchronously
            
                // assert
                Expect.equal result entries ""

            testCase "GetSingle: returns None when no results match"
            <| fun _ ->
                // arrange
                let slug = "blah-blah"
            
                let context = EmptyContext()
            
                let target = BlogRepository(context) :> IRepository<BlogEntry>
            
                // act
                let result =
                    target.GetSingle slug
                    |> Async.RunSynchronously
            
                // assert
                Expect.isNone result ""
            
            testCase "GetSingle: returns Some if more than one result"
            <| fun _ ->
                // arrange
                let expected = BlogEntry.create "one"
            
                let entries =
                    [
                        expected
                    ]
            
                let context = A.Fake<IContext<BlogEntry>>()
                A.CallTo(fun () -> context.All())
                    .Returns(entries |> Seq.ofList |> Task.FromResult) |> ignore
            
                let target = BlogRepository(context) :> IRepository<BlogEntry>
            
                // act
                let result =
                    target.GetSingle expected.Slug
                    |> Async.RunSynchronously
            
                // assert
                match result with
                | None -> failtest "Should return Some result."
                | Some r -> Expect.equal r expected ""
            
            testCase "Update: returns None if slug not found"
            <| fun _ ->
                // arrange
                let context = EmptyContext()
            
                let target = BlogRepository(context) :> IRepository<BlogEntry>
            
                // act
                let result =
                    target.Update (BlogEntry.create "I Don't Exist")
                    |> Async.RunSynchronously
            
                // assert
                Expect.isNone result ""
            
            testCase "Update: returns updated result when present"
            <| fun _ ->
                // arrange
                let expected = BlogEntry.create "one"
            
                let context =
                    Mock<IContext<BlogEntry>>()
                        .Setup(fun r -> <@ r.Update expected @>)
                        .Returns(1 |> Task.FromResult)
                        .Create()
            
                let target = BlogRepository(context) :> IRepository<BlogEntry>
            
                // act
                let result =
                    target.Update expected
                    |> Async.RunSynchronously
            
                // assert
                match result with
                | None -> failtest "Should return Some result."
                | Some e -> Expect.equal e expected ""
        ]
        
let songRepoTests =
    testList
        "Song Repository"
        [

            testCase "GetAll: returns all published songs in context."
            <| fun _ ->
                // arrange
                let songs =
                    [
                        { Song.create "one" with IsPublished = true }
                        { Song.create "two" with IsPublished = true }
                        { Song.create "three" with IsPublished = false }
                        { Song.create "three" with IsPublished = true }
                    ]

                let context =
                    A.Fake<IContext<Song>>()
                    
                A.CallTo(fun () -> context.All()).Returns(songs |> Seq.ofList |> Task.FromResult) |> ignore

                let target = SongRepository(context) :> IRepository<Song>

                // act
                let result =
                    target.GetAll()
                    |> Async.RunSynchronously

                // arrange assert
                let expected =
                    songs
                    |> List.filter (fun s -> s.IsPublished)
                // assert
                Expect.equal result expected ""

            testCase "GetSingle: returns None when no results match"
            <| fun _ ->
                // arrange
                let slug = "blah-blah"
            
                let context = EmptyContext()
            
                let target = SongRepository(context) :> IRepository<Song>
            
                // act
                let result =
                    target.GetSingle slug
                    |> Async.RunSynchronously
            
                // assert
                Expect.isNone result ""
            
            testCase "GetSingle: returns Some if more than one result"
            <| fun _ ->
                // arrange
                let expected = Song.create "one"
            
                let songs =
                    [
                        expected
                    ]
            
                let context = A.Fake<IContext<Song>>()
                A.CallTo(fun () -> context.All()).Returns(songs |> Seq.ofList |> Task.FromResult) |> ignore
                
                let target = SongRepository(context) :> IRepository<Song>
            
                // act
                let result =
                    target.GetSingle expected.Slug
                    |> Async.RunSynchronously
            
                // assert
                match result with
                | None -> failtest "Should return Some result."
                | Some r -> Expect.equal r expected ""
            
            testCase "Update: returns None if slug not found"
            <| fun _ ->
                // arrange
                let context = EmptyContext()
            
                let target = SongRepository(context) :> IRepository<Song>
            
                // act
                let result =
                    target.Update (Song.create "I Don't Exist")
                    |> Async.RunSynchronously
            
                // assert
                Expect.isNone result ""
            
            testCase "Update: returns updated result when present"
            <| fun _ ->
                // arrange
                let expected = Song.create "one"
            
                let context = A.Fake<IContext<Song>>()
                A.CallTo(fun () -> context.Update expected).Returns(1 |> Task.FromResult) |> ignore
            
                let target = SongRepository(context) :> IRepository<Song>
            
                // act
                let result =
                    target.Update expected
                    |> Async.RunSynchronously
            
                // assert
                match result with
                | None -> failtest "Should return Some result."
                | Some e -> Expect.equal e expected ""
        ]
        
let all =
    testList
        "Repository"
        [
            blogRepoTests
            songRepoTests
        ]