module Build.Configuration

open Fake.IO
open Microsoft.Extensions.Configuration

let private currentDirectory = Path.combine (System.IO.Directory.GetCurrentDirectory()) "Build"
let private builder =
    ConfigurationBuilder()
        .SetBasePath(currentDirectory)
        .AddJsonFile("appsettings.json", optional = false, reloadOnChange = true)

let configuration : IConfiguration = builder.Build()
