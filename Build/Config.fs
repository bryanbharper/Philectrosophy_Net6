module Build.Config

open Fake.IO
open Microsoft.Extensions.Configuration

let private currentDirectory = Path.combine (System.IO.Directory.GetCurrentDirectory()) "Build"
let private builder =
    ConfigurationBuilder()
        .SetBasePath(currentDirectory)
        .AddJsonFile("appsettings.json", optional = false, reloadOnChange = true)

let configuration : IConfiguration = builder.Build()

module Constants =
    let appName = "philectrosophy"
    
module Paths =
    let shared = Path.getFullName "Shared"
    let server = Path.getFullName "Server"
    let client = Path.getFullName "Client"
    let deploy = Path.getFullName "deploy"

    let blogImage =
        Path.combine server "public/blog.posts/img"
    let clientPublic = Path.combine client "public"

    let sharedTests = Path.getFullName "Tests.Shared"
    let serverTests = Path.getFullName "Tests.Server"
    // let clientTestsPath = Path.getFullName "Tests.Client"