module Build.Main

open Fake.Core
open Fake.IO
open Farmer
open Farmer.Builders

open Helpers

initializeContext()

let sharedPath = Path.getFullName "Shared"
let serverPath = Path.getFullName "Server"
let clientPath = Path.getFullName "Client"
let deployPath = Path.getFullName "deploy"

let blogImagePath =
    Path.combine serverPath "public/blog.posts/img"
let clientPublicPath = Path.combine clientPath "public"

let sharedTestsPath = Path.getFullName "Tests.Shared"
let serverTestsPath = Path.getFullName "Tests.Server"
// let clientTestsPath = Path.getFullName "Tests.Client"

let printSection msg =
    Trace.traceLine ()
    Trace.tracefn $"%s{msg}"
    Trace.traceLine ()

Target.create "Clean" (fun _ ->
    Shell.cleanDir deployPath
    run dotnet "fable clean --yes" clientPath // Delete *.fs.js files created by Fable
)

Target.create "BlogImages"
<| fun _ ->
    "Moving blog images to client." |> printSection
    Shell.copyDir (Path.combine clientPublicPath "img") blogImagePath (fun _ -> true)

Target.create "InstallClient" (fun _ -> run npm "install" clientPath)

Target.create "Bundle" (fun _ ->
    [ "server", dotnet $"publish -c Release -o \"{deployPath}\"" serverPath
      "client", dotnet "fable -o output -s --run npm run build" clientPath ]
    |> runParallel
)

Target.create "Azure" (fun _ ->
    let web = webApp {
        name "Safe4._2._0"
        operating_system OS.Windows
        runtime_stack Runtime.DotNet60
        zip_deploy "deploy"
    }
    let deployment = arm {
        location Location.WestEurope
        add_resource web
    }

    deployment
    |> Deploy.execute "Safe4._2._0" Deploy.NoParameters
    |> ignore
)

Target.create "Run" (fun _ ->
    run dotnet "build" sharedPath
    [ "server", dotnet "watch run" serverPath
      "client", dotnet "fable watch -o output -s --run npm run start" clientPath ]
    |> runParallel
)

Target.create "RunTests" (fun _ ->
    run dotnet "build" sharedTestsPath
    [
        "server", dotnet "watch run" serverTestsPath
        // "client", dotnet "fable watch -o output -s --run npm run test:live" clientTestsPath
    ]
    |> runParallel
)

open Fake.Core.TargetOperators

let dependencies = [
    "Clean"
        ==> "InstallClient"
        ==> "Bundle"
        ==> "Azure"

    "Clean"
        ==> "InstallClient"
        ==> "BlogImages"
        ==> "Run"

    "InstallClient"
        ==> "RunTests"
]

[<EntryPoint>]
let main args = runOrDefault args