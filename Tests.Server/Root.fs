module Tests.Server.Root

open Expecto
open Tests

let allTests =
    testList "Server Tests"
        [
            Data.Repository.all
            Api.BlogApi.all
            Api.SongApi.all
            File.all
            Markdown.all
            Rank.all
            Shared.Root.all
        ]

[<EntryPoint>]
let main _ = runTests defaultConfig allTests
