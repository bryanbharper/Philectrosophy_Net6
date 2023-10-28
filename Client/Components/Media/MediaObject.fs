module Client.Components.MediaObject

open Client.Styles
open Feliz

let render imgUrl (mediaRight: ReactElement list) (mainContents: ReactElement list) =
    Html.div [
        prop.className Bulma.Media
        prop.children [
            Html.div [
                prop.className Bulma.MediaLeft
                prop.children [
                    Html.figure [
                        prop.classes [ Bulma.Image; Bulma.Is96X96 ]
                        prop.children [
                            Html.img [ prop.src imgUrl ]
                        ]
                    ]
                ]
            ]

            Html.div [
                prop.className Bulma.MediaContent
                prop.children [
                    Html.div [
                        prop.className Bulma.Content
                        prop.children mainContents
                    ]
                ]
            ]
            
            Html.div [
                prop.className Bulma.MediaRight
                prop.children mediaRight
            ]
        ]
    ]