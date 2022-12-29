module Client.Pages.BaseError

open Client.Styles
open Feliz

let private layout (contents: ReactElement) =
    Html.section [
        Html.div [
            prop.className Bulma.Columns
            prop.children [
                Html.div [
                    prop.classes [ Bulma.Column; Bulma.IsOffsetOneQuarter; Bulma.IsHalf ]
                    prop.children contents
                ]
            ]
        ]
    ]

let private messageBlock color (headMsg: string) (bodyMsg: string) =
    Html.article [
        prop.className Bulma.Message
        prop.children [
            Html.div [
                prop.className Bulma.MessageHeader
                prop.text headMsg
            ]
            Html.div [
                prop.className Bulma.MessageBody
                prop.text bodyMsg
            ]
        ]
    ]

let private imageBlock src =
    Html.div [
        prop.className Bulma.HasTextCentered
        prop.children [
            Html.figure [
                prop.classes [ Bulma.Image; Bulma.IsInlineBlock ]
                prop.children [
                    Html.img [ prop.src src ]
                ]
            ]
        ]
    ]

let render color headerMsg bodyMsg imgSrc =
    Html.section [
        prop.classes [ Bulma.Section ]
        prop.children [
            messageBlock color headerMsg bodyMsg |> layout
            imageBlock imgSrc
        ]
    ]