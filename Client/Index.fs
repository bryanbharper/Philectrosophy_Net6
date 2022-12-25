﻿module Index

open Elmish
open Fable.Remoting.Client
open Shared

type Model = { Todos: Todo list; Input: string }

type Msg =
    | GotTodos of Todo list
    | SetInput of string
    | AddTodo
    | AddedTodo of Todo

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let init () : Model * Cmd<Msg> =
    let model = { Todos = []; Input = "" }

    let cmd = Cmd.OfAsync.perform todosApi.getTodos () GotTodos

    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | GotTodos todos -> { model with Todos = todos }, Cmd.none
    | SetInput value -> { model with Input = value }, Cmd.none
    | AddTodo ->
        let todo = Todo.create model.Input

        let cmd = Cmd.OfAsync.perform todosApi.addTodo todo AddedTodo

        { model with Input = "" }, cmd
    | AddedTodo todo -> { model with Todos = model.Todos @ [ todo ] }, Cmd.none

open Feliz

let navBrand =
    Html.div [
        prop.className "navbar-brand"
        prop.children [
            Html.a [
                prop.className "navbar-item"
                prop.href "#"
                prop.children [
                    Html.img [
                        prop.src "https://bulma.io/images/bulma-logo.png"
                        prop.width 112
                        prop.height 28
                    ]
                ]
            ]
            
        ]
    ]

let containerBox (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "box"
        prop.children [
            Html.ol [
                for todo in model.Todos do
                    Html.li [ prop.text todo.Description ]
            ]
            Html.div [
                prop.className "field is-grouped"
                prop.children [
                    Html.div [
                        prop.className "control is-expanded"
                        prop.children [
                            Html.input [
                                prop.className "input"
                                prop.value model.Input
                                prop.placeholder "What needs to be done?"
                                prop.onChange (fun x -> SetInput x |> dispatch)
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className "control"
                        prop.children [
                            Html.a [
                                prop.className "button is-primary"
                                prop.disabled (Todo.isValid model.Input |> not)
                                prop.onClick (fun _ -> dispatch AddTodo)
                                prop.text "Add"
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Html.section [
        prop.classes [ "hero"; "is-fullheight"; "is-primary" ]
        prop.style [
            style.backgroundSize "cover"
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
            style.backgroundPosition "no-repeat center center fixed"
        ]
        prop.children [
            Html.div [
                prop.className "hero-head"
                prop.children [
                    Html.nav [
                        prop.className "navbar"
                        prop.children [
                            Html.div [
                                prop.className "container"
                                prop.children [
                                    navBrand
                                ]
                            ]
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.className "hero-body"
                prop.children [
                    Html.div [
                        prop.className "container"
                        prop.children [
                            Html.div [
                                prop.className "column is-6 is-offset-3"
                                prop.children [
                                    Html.h1 [
                                        prop.className "title has-text-centered"
                                        prop.text "Safe4._2._0"
                                    ]
                                    containerBox model dispatch
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]