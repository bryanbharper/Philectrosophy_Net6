module Client.Pages.Blog

open Fable.Remoting.Client
open Elmish
open Elmish.Navigation // install Fable.Elmish.Browser instead of Elmish.Navigation
open Feliz
open Feliz.Router

open Shared.Types
open Shared.Dtos
open Shared.Contracts

open Client.Styles
open Client.Components
open Client.Urls

let blogApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IBlogApi>

type State = { Entries: Deferred<BlogEntry list> }

type Msg =
    | ServerReturnedEntries of BlogEntry list
    | ServerReturnedError of exn

let init (): State * Cmd<Msg> =
    { Entries = InProgress }, Cmd.OfAsync.either blogApi.GetEntries () ServerReturnedEntries ServerReturnedError

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match msg with
    | ServerReturnedEntries result -> { state with Entries = Resolved result }, Cmd.none
    | ServerReturnedError _ -> state, Url.UnexpectedError.asString |> Navigation.newUrl

let render (state: State) (_: Msg -> unit) =
    let entries =
        match state.Entries with
        | Idle -> Html.none
        | InProgress -> Spinner.render
        | Resolved entries ->
            entries
            |> List.map EntryMedia.render
            |> Html.div

    Html.section [
        prop.className Bulma.Section
        prop.children [
            Html.div [
                prop.classes [ Bulma.Container ]
                prop.children [
                    Html.h1 [
                        prop.text "Blog"
                        prop.classes [
                            Bulma.Title
                            Bulma.IsSize1
                            Bulma.IsSize2Desktop
                        ]
                    ]
                    Html.hr []
                    entries
                ]
            ]
        ]
    ]

