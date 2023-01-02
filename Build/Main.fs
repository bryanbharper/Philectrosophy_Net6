module Build.Main

open Build.Config
open Fake.Core
open Fake.IO
open Farmer
open Farmer.Builders

open Helpers

Context.initialize()

let printSection msg =
    Trace.traceLine ()
    Trace.tracefn $"%s{msg}"
    Trace.traceLine ()

Target.create "Clean" (fun _ ->
    Shell.cleanDir Paths.deploy
    run dotnet "fable clean --yes" Paths.client // Delete *.fs.js files created by Fable
)

Target.create "BlogImages"
<| fun _ ->
    "Moving blog images to client." |> printSection
    Shell.copyDir (Path.combine Paths.clientPublic "img") Paths.blogImage (fun _ -> true)

Target.create "InstallClient" (fun _ -> run npm "install" Paths.client)

Target.create "Bundle" (fun _ ->
    [ "server", dotnet $"publish -c Release -o \"{Paths.deploy}\"" Paths.server
      "client", dotnet "fable -o output -s --run npm run build" Paths.client ]
    |> runParallel
)

Target.create "WriteSitemap"
<| fun _ ->
    "Writing sitemap.xml" |> printSection
    SiteMap.write ()

Target.create "Sandbox"
<| fun _ ->
    "Deploying to Sandbox Environment on Azure" |> printSection

    let web =
        webApp {
            name (Constants.appName + "-sbx")
            operating_system OS.Windows
            runtime_stack Runtime.DotNet60
            zip_deploy Paths.deploy
            setting "ASPNETCORE_ENVIRONMENT" "Sandbox"
        }

    let deployment =
        arm {
            location Location.CentralUS
            add_resource web
        }

    deployment
    |> Deploy.execute Constants.appName Deploy.NoParameters
    |> ignore

Target.create "Deploy" (fun _ ->
    let web = webApp {
        name Constants.appName
        sku WebApp.Sku.D1
        operating_system OS.Windows
        runtime_stack Runtime.DotNet60
        zip_deploy Paths.deploy
    }
    
    let deployment = arm {
        location Location.CentralUS
        add_resource web
    }

    deployment
    |> Deploy.execute Constants.appName Deploy.NoParameters
    |> ignore
)

Target.create "Run" (fun _ ->
    run dotnet "build" Paths.shared
    [ "server", dotnet "watch run" Paths.server
      "client", dotnet "fable watch -o output -s --run npm run start" Paths.client ]
    |> runParallel
)

Target.create "Test" (fun _ ->
    run dotnet "build" Paths.sharedTests
    [
        "server", dotnet "watch run" Paths.serverTests
        // "client", dotnet "fable watch -o output -s --run npm run test:live" clientTestsPath
    ]
    |> runParallel
)

open Fake.Core.TargetOperators

let dependencies = [
    "BlogImages"
        ==> "Bundle"
    
    "WriteSitemap"
        ==> "Bundle"
    
    "Clean"
        ==> "InstallClient"
        ==> "Bundle"
        ==> "Deploy"
        
    "Clean"
        ==> "InstallClient"
        ==> "Bundle"
        ==> "Sandbox"

    "Clean"
        ==> "InstallClient"
        ==> "BlogImages"
        ==> "Run"

    "InstallClient"
        ==> "Test"
]

[<EntryPoint>]
let main args = runOrDefault args