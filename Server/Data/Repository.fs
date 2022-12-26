namespace Server.Data

open Shared.Dtos
open Shared.Extensions
 
type IRepository<'a> =
    abstract GetAll: unit -> Async<'a list>
    abstract GetSingle: slug:string -> Async<'a option>
    abstract Update: updatedEntry:'a -> Async<'a option>

type BlogRepository(context: IContext<BlogEntry>) =
    interface IRepository<BlogEntry> with
        member this.GetAll() =
            context.All()
            |> Async.AwaitTask
            |> Async.map List.ofSeq
            
        member this.GetSingle slug =
            context.Where (fun entry -> entry.Slug = slug)
            |> Async.AwaitTask
            |> Async.map (fun r -> if (Seq.length r) > 0 then r |> Seq.head |> Some else None)

        member this.Update entry =
            async {
                let! rowsAffected =
                    context.Update entry
                    |> Async.AwaitTask
                
                return if rowsAffected > 0 then Some entry else None
            }
            
type SongRepository(context: IContext<Song>) =
    interface IRepository<Song> with
        member this.GetAll() =
            context.All()
            |> Async.AwaitTask
            |> Async.map List.ofSeq
            
        member this.GetSingle slug =
            context.Where (fun entry -> entry.Slug = slug)
            |> Async.AwaitTask
            |> Async.map (fun r -> if (Seq.length r) > 0 then r |> Seq.head |> Some else None)

        member this.Update entry =
            async {
                let! rowsAffected =
                    context.Update entry
                    |> Async.AwaitTask
                
                return if rowsAffected > 0 then Some entry else None
            }
