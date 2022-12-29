module Index

open Client.Components
open Client.Urls
open Elmish
open Client.Styles

type Model =
    {
        Number: int
        Navbar: Navbar.State
    }

type Msg =
    | ButtonWasClicked
    | Navbar of Navbar.Msg

let init () : Model * Cmd<Msg> =
    let model =
        {
            Number = 0
            Navbar = Url.Blog |> Some |> Navbar.init
        }

    model, Cmd.none

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | ButtonWasClicked ->
        { model with Number = model.Number + 1 }, Cmd.none
    | Navbar msg' ->
        { model with
            Navbar = Navbar.update msg' model.Navbar
        },
        Cmd.none

open Feliz

let view (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        Navbar.render model.Navbar (Msg.Navbar >> dispatch)
        Html.section [
            prop.classes [ Bulma.Section ]
            prop.children [
                Html.h1 [
                    prop.classes [ Bulma.Title ]
                    prop.text $"{model.Number}"
                ]
                Html.button [
                    prop.classes [ Bulma.Button ]
                    prop.text "Click me"
                    prop.onClick (fun _ -> dispatch ButtonWasClicked)
                ]
            ]
        ]
    ]
