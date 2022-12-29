﻿module Client.Pages.BlogEntry


open Elmish
open Elmish.Navigation
open Fable.Remoting.Client
open Feliz

open Shared.Types
open Shared.Extensions
open Shared.Dtos
open Shared.Contracts

open Client.Components
// open Client.Components.ReactMarkdown
open Client.Styles
open Client.Urls

let blogApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IBlogApi>

type Entry =
    {
        Metadata: BlogEntry
        Content: string
    }

type State =
    {
        Slug: string
        Entry: Deferred<Entry>
    }

type Msg =
    | ServerReturnedError of exn
    | ServerUpdatedViewCount of int option
    | ServerReturnedEntry of Option<BlogEntry * string>
    | MetaTagsUpdated of unit

let init (slug: string): State * Cmd<Msg> =
    { Slug = slug; Entry = InProgress }, Cmd.OfAsync.either blogApi.GetEntry slug ServerReturnedEntry ServerReturnedError

open Browser

let setMetaTags (entry: BlogEntry) =
    document
        .querySelector("meta[property=\"og:title\"]")
        .setAttribute("content", entry.Title)

    document
        .querySelector("meta[property=\"og:description\"]")
        .setAttribute("content", entry.Synopsis)

    document
        .querySelector("meta[property=\"og:image\"]")
        .setAttribute("content", "http://www.philectrosophy.com/" + entry.ThumbNailUrl)

    document
        .querySelector("meta[property=\"og:url\"]")
        .setAttribute("content", "http://www.philectrosophy.com/blog/" + entry.Slug)

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match msg with
    | ServerReturnedError _ -> state, Url.UnexpectedError.asString |> Navigation.newUrl
    | ServerUpdatedViewCount None -> state, Cmd.none
    | ServerUpdatedViewCount (Some newCount) ->
        let newState =
            match state.Entry with
            | Idle -> state
            | InProgress -> state
            | Resolved entry ->
                let meta =
                    { entry.Metadata with
                        ViewCount = newCount
                    }

                let entry = { entry with Metadata = meta }
                { state with Entry = entry |> Resolved }

        newState, Cmd.none
    | ServerReturnedEntry None -> state, Url.NotFound.asString |> Navigation.newUrl
    | ServerReturnedEntry (Some (metadata, content)) ->
        let entry =
            {
                Metadata = metadata
                Content = content
            }

        { state with Entry = Resolved entry },
        Cmd.batch [
            Cmd.OfFunc.perform setMetaTags entry.Metadata MetaTagsUpdated
            Cmd.OfAsync.perform blogApi.UpdateViewCount state.Slug ServerUpdatedViewCount
        ]
    | MetaTagsUpdated _ -> state, Cmd.none

let dateHeader metadata =
    let updatedMsg =
        match metadata.UpdatedOn with
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

    Html.p [
        prop.classes [
            Bulma.Subtitle
            Bulma.Is6
            Bulma.Mb1
        ]
        prop.children [
            Html.span [
                prop.classes [
                    Bulma.HasTextGreyLight
                    if metadata.UpdatedOn.IsSome then Style.IsStrikeThrough else Bulma.IsItalic
                ]
                prop.text $"Posted: %s{Date.format metadata.CreatedOn}"
            ]
            updatedMsg
        ]
    ]


let viewCount count =
    Html.span [
        prop.classes [ Bulma.HasTextGreyLight ]
        prop.children [
            Html.i [
                prop.classes [ Bulma.Icon; FA.Fas; FA.FaEye ]
            ]
            Html.text $"%i{count}"
        ]
    ]


let header (metadata: BlogEntry) =
    [
        Html.h2 [
            prop.classes [ Bulma.Title ]
            prop.text metadata.Title
        ]

        match metadata.Subtitle with
        | None -> Html.none
        | Some subtitle ->
            Html.h4 [
                prop.classes [
                    Bulma.Subtitle
                    Bulma.HasTextGrey
                    Bulma.Mb1
                ]
                prop.text subtitle
            ]

        dateHeader metadata
        viewCount metadata.ViewCount
        Html.hr []
    ]
    |> Html.div

let layout (contents: ReactElement list) =
    Html.section [
        prop.classes [
            Bulma.Section
        ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.Columns ]
                prop.children [
                    Html.div [
                        prop.classes [
                            Bulma.Column
                            Bulma.IsOneQuarterWidescreen
                            Bulma.IsOneQuarterDesktop
                        ]
                    ]
                    Html.div [
                        prop.classes [
                            Bulma.Column
                            Bulma.IsHalfWidescreen
                            Bulma.IsHalfDesktop
                            Bulma.IsFullMobile
                            Bulma.IsFullTablet
                        ]
                        prop.children contents
                    ]
                ]
            ]
        ]
    ]


let render (state: State) (_: Msg -> unit): ReactElement =
    layout [
        match state.Entry with
        | Idle -> Html.none
        | InProgress -> Spinner.render
        | Resolved entry ->
            header entry.Metadata
            Html.h1 "TODO: Render markdown"
            // Markdown.render [
            //     markdown.children entry.Content
            //     markdown.linkTarget "_blank"
            //     markdown.className Style.Markdown
            //     markdown.rehypePlugins [| Rehype.raw |]
            // ]
            Html.hr []
            // Disqus.render entry.Metadata.Slug entry.Metadata.Title
    ]
