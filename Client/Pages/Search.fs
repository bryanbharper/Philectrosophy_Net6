module Client.Pages.Search

open Elmish
open Fable.Remoting.Client
open Feliz

open Shared.Types
open Shared.Dtos
open Shared.Contracts

open Client.Styles
open Client.Components


let blogApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<IBlogApi>

type State =
    {
        Query: string option
        Results: Deferred<BlogEntry list>
    }

type Msg =
    | ServerReturnedBlogEntries of BlogEntry list
    | ServerReturnedError of exn
    | UserChangedInput of string
    | UserClearedSearch
    | UserClickedSubmit

let init (): State * Cmd<Msg> =
    { Query = None; Results = Idle }, Cmd.none

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match msg with
    | ServerReturnedError _ -> { state with Results = Resolved [] }, Cmd.none
    | ServerReturnedBlogEntries entries ->
        { state with
            Results = Resolved entries
        },
        Cmd.none
    | UserChangedInput query ->
        match query with
        | "" ->
            { state with Results = Idle; Query = None }, Cmd.none
        | _ ->
            { state with Query = Some query }, Cmd.none
    | UserClearedSearch ->
        { state with Results = Idle }, Cmd.none
    | UserClickedSubmit ->
        match state.Query with
        | None -> state, Cmd.none
        | Some query ->
            { state with Results = InProgress },
            Cmd.OfAsync.either blogApi.GetSearchResults query ServerReturnedBlogEntries ServerReturnedError

let input dispatch =
    Html.input [
        prop.classes [
            Bulma.Input
            Bulma.IsRounded
        ]
        prop.text "search"
        prop.onChange (UserChangedInput >> dispatch)
        prop.onKeyDown (fun ke -> if ke.key = "Enter" then dispatch UserClickedSubmit)
        prop.onEmptied (fun _ -> dispatch UserClearedSearch)
    ]


let render (state: State) (dispatch: Msg -> unit): ReactElement =
    let results =
        match state.Results with
        | Idle -> Html.none
        | InProgress -> Spinner.render
        | Resolved results' ->
            match results' with
            | [] ->
                Html.h4 [
                    prop.classes [ Bulma.Subtitle ]
                    prop.text "No results found..."
                ]
            | _ ->
                results'
                |> List.map EntryMedia.render
                |> Html.div

    Html.section [
        prop.classes [ Bulma.Section ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.Container ]
                prop.children [
                    Html.h2 [
                        prop.classes [ Bulma.Title ]
                        prop.text "Search"
                    ]
                    Html.nav [
                        prop.classes [ Bulma.Level ]
                        prop.children [
                            input dispatch
                        ]
                    ]
                    results
                ]
            ]
        ]
    ]

