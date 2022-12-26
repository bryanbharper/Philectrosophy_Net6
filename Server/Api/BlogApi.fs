namespace Server.Api

open Shared.Contracts
open Shared.Dtos
open Shared.Extensions

open Server
open Server.Data
open Server.File

module BlogApi =
    let getEntriesAsync (repo: IRepository<BlogEntry>) =
        repo.GetAll()
        |> Async.map
            (List.filter (fun e -> e.IsPublished)
             >> List.sortByDescending (fun e -> e.CreatedOn))

    let getEntryAsync (repo: IRepository<BlogEntry>) (fileStore: IBlogContentStore) slug =
        (repo.GetSingle slug, fileStore.GetBlogEntryContentAsync slug)
        |> Tuple.sequenceAsync
        |> Async.map Tuple.sequenceOption

    let getSearchResults (repo: IRepository<BlogEntry>) query =
        repo.GetAll()
        |> Async.map (List.filter (fun e -> e.IsPublished) >> Rank.entries query)

    let updateViewCount (repo: IRepository<BlogEntry>) slug =
        async {
            let! entryOp = repo.GetSingle slug

            return!
                entryOp
                |> Option.map (fun e -> repo.Update { e with ViewCount = e.ViewCount + 1 })
                |> Option.sequenceAsync
                |> Async.map
                    (Option.flatten
                     >> Option.map (fun e -> e.ViewCount))
        }

    let blogApiReader =
        reader {
            let! repo = resolve<IRepository<BlogEntry>> ()
            let! fileStore = resolve<IBlogContentStore> ()

            return
                {
                    GetEntries = fun () -> getEntriesAsync repo
                    GetEntry = getEntryAsync repo fileStore
                    GetSearchResults = getSearchResults repo
                    UpdateViewCount = updateViewCount repo
                }
        }
