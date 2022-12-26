namespace Server.Api

open Server.Data
open Shared.Dtos
open Shared.Extensions
open Shared.Contracts

module SongApi =
    let getSongsAsync (repo: IRepository<Song>) =
        repo.GetAll()

    let updateListenCount (repo: IRepository<Song>) slug =
        async {
            let! songOption = repo.GetSingle slug

            return!
                songOption
                |> Option.map (fun e -> repo.Update { e with PlayCount = e.PlayCount + 1 })
                |> Option.sequenceAsync
                |> Async.map Option.flatten
        }

    let songApiReader =
        reader {
            let! repo = resolve<IRepository<Song>> ()

            return
                {
                    GetSongs = fun () -> getSongsAsync repo
                    UpdateListenCount = updateListenCount repo
                }
        }


