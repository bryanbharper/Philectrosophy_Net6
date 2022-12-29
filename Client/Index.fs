module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Todo = { Id: int; Text: string; IsCompleted: bool }

type Model = { Todos: Todo list; Input: string }

type Msg = unit

let init () : Model * Cmd<Msg> =
    let model = { Todos = []; Input = "" }

    model, Cmd.none

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | _ -> model, Cmd.none

open Feliz

let view (model: Model) (dispatch: Msg -> unit) =
    Html.h1 "Hello world"