module Build.SiteMap

open System.IO
open Shared.Dtos
open Shared.Extensions
open Server.Data

let private getIds () =
    let context = BlogContext(Config.configuration)
    let repo = BlogRepository(context) :> IRepository<BlogEntry>
    repo.GetAll()
    |> Async.map (fun entries -> entries |> List.map (fun entry -> entry.Slug))
    |> Async.RunSynchronously
    

let private writeSitemap (ids: string list) =
    let urlNodes =
        ids
        |> List.map (fun id -> $"\t<url><loc>https://www.philectrosophy.com/blog/%s{id}</loc></url>")
        |> String.concat "\n"
    
    let sitemap =
        $"<?xml version='1.0' encoding='UTF-8'?>\n<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>\n%s{urlNodes}\n</urlset>"
    
    let filePath = Path.Combine("Client/public", "sitemap.xml")
    File.WriteAllText(filePath, sitemap)

let write () =
    let ids = getIds()
    writeSitemap ids