module Client.Components.EntryMedia

open Feliz

open Feliz.Bulma.Bulma
open Shared.Extensions
open Shared.Dtos

open Client.Styles
open Client.Urls

let private title (entry: BlogEntry) = 
    Html.h4 [ 
        prop.classes [ Bulma.Title; Bulma.Is4 ]
        prop.text entry.Title 
    ]


let private updatedMsg entry =
    match entry.UpdatedOn with
    | None -> Html.none
    | Some date ->
        Html.span [
            prop.classes [
                Bulma.HasTextGrey
                Bulma.IsItalic
                Bulma.Ml1
            ]
            prop.text $"Updated: %s{Date.format date}"
        ]

let private subtitle entry =
    Html.div [
        prop.classes [ Bulma.Subtitle; Bulma.Is5; Bulma.Mb1 ]
        prop.children [
            Html.span [
                prop.classes [
                    Bulma.HasTextGrey
                ]
                
                match entry.Subtitle with
                | Some subtitle -> subtitle
                | _ -> ""
                |> prop.text
            ]
        ]
    ]

let private date entry =
    Html.p [
        prop.classes [ Bulma.Subtitle; Bulma.Is6; Bulma.Mb1 ]
        prop.children [
            Html.span [
                prop.classes [
                    Bulma.HasTextGreyLight
                    if entry.UpdatedOn.IsSome then Style.IsStrikeThrough else Bulma.IsItalic
                ]
                prop.text $"Posted the %s{Date.format entry.CreatedOn}"
            ]
            updatedMsg entry
        ]
    ]

let private views viewCount =
     Html.span [
         prop.classes [ "icon-text"; Bulma.HasTextGreyLight ]
         prop.children [
             Html.span [
                prop.classes [ Bulma.Icon ]
                prop.children [
                    Html.i [
                        prop.classes [ FA.Fa; FA.FaEye ]
                    ]
                ]
             ]
             Html.span $"%d{viewCount}"
         ]
     ]
     
let subTitleAndViews entry =
    Html.div [
        prop.classes [ Bulma.Subtitle ]
        prop.children [
            views entry.ViewCount
            subtitle entry
        ]
    ]

let private synopsis entry = Html.p entry.Synopsis

let private media (entry: BlogEntry) =
    [
        title entry
        // subtitle entry
        subTitleAndViews entry
        date entry
        synopsis entry
    ]
    |> MediaObject.render entry.ThumbNailUrl [ ]

let private (</>) left right = $"%s{left}/%s{right}"

let render (entry: BlogEntry) =
    Html.div [
        prop.classes [ Bulma.Mb6 ]

        prop.children [
            Html.a [
                prop.classes [ Style.SecretAnchor ]
                Url.Blog.asString </> entry.Slug |> prop.href
                entry |> media |> prop.children
            ]
        ]
    ]
