module Server.Tests.All

open Expecto

let allTests =
    testList "|Server Tests|"
        [
            // Data.BlogRepository.all
            // Data.SongRepository.all
            // Api.BlogApi.all
            // Api.SongApi.all
            // File.all
            // Markdown.all
            // Rank.all
            Tests.Shared.Root.allTests
        ]

[<EntryPoint>]
let main _ = runTests defaultConfig allTests
