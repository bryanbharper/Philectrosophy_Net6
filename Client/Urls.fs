module Client.Urls

open Microsoft.FSharp.Reflection

[<RequireQualifiedAccess>]
type Url =
    | About
    | Blog
    | BlogEntry of slug: string
    | Music
    | Track of slug: string
    | NotFound
    | Search
    | UnexpectedError
    member this.asString =
        let this' =
            match this with
            | BlogEntry _ -> Blog
            | Track _ -> Music
            | _ -> this

        let case, _ =
            FSharpValue.GetUnionFields(this', typeof<Url>)

        case.Name.ToLower()

module Url =
    let toString (url: Url) = url.asString
