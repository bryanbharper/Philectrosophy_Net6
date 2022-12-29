module Client.Components.Navbar

open Client.Styles
open Client.Urls
open Feliz

type State =
    {
        BurgerExpanded: bool
        ActivePage: Url
    }

type Msg =
    | UserClickedBurger
    | UrlChanged of Url

let init (url: Option<Url>): State =
    match url with
    | None ->
        {
            ActivePage = Url.Blog
            BurgerExpanded = false
        }
    | Some url ->
        {
            BurgerExpanded = false
            ActivePage = url
        }

let update (msg: Msg) (state: State): State =
    match msg with
    | UserClickedBurger ->
        { state with
            BurgerExpanded = not state.BurgerExpanded
        }
    | UrlChanged url -> { state with ActivePage = url }

let navLink (url: Url) isActive =
    Html.a [
        prop.classes [
            Bulma.NavbarItem
            if isActive then Bulma.IsActive
        ]
        prop.href (url |> Url.toString)
        prop.text (url |> Url.toString)
    ]

let navLinkIcon (url: Url) isActive fontAwesomeIconName =
    Html.a [
        prop.classes [
            Bulma.NavbarItem
            if isActive then Bulma.IsActive
        ]
        url |> Url.toString |> prop.href
        prop.children [
            Html.span [
                prop.classes [
                    Bulma.Icon
                ]
                prop.children [
                    Html.i [
                        prop.classes [
                            FA.Fas
                            fontAwesomeIconName
                        ]
                    ]
                ]
            ]
        ]
    ]

let render (state: State) (dispatch: Msg -> unit): ReactElement =    
    Html.nav [
        prop.classes [ Bulma.Navbar; Bulma.IsFixedTop; Bulma.IsWhite ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.NavbarBrand ]
                prop.children [
                    Html.a [
                        prop.classes [ Bulma.NavbarItem; Style.Logo ]
                        prop.href ""
                        prop.children [
                            Html.img [ prop.src "phi.png" ]
                            Html.p [
                                prop.classes [ Style.LogoText; Bulma.IsSize5; Bulma.Px1 ]
                                prop.innerHtml "&#295;i&#8467;&epsilon;c&tau;&#8477;&sigma;&#8747;&theta;&rho;&#295;&gamma;"
                            ]
                        ]
                    ]
                    Html.a [
                        prop.classes [ Bulma.NavbarBurger; if state.BurgerExpanded then Bulma.IsActive ]
                        prop.onClick (fun _ -> Msg.UserClickedBurger |> dispatch)
                        prop.children [
                            Html.span []
                            Html.span []
                            Html.span []
                            Html.span []
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.classes [ Bulma.NavbarMenu; if state.BurgerExpanded then Bulma.IsActive ]
                prop.children [
                    Html.div [
                        prop.classes [ Bulma.NavbarEnd ]
                        prop.children [
                            navLink Url.Blog (state.ActivePage = Url.Blog)
                            navLink Url.About (state.ActivePage = Url.About)
                            navLinkIcon Url.Music (state.ActivePage = Url.Music) FA.FaMusic
                            navLinkIcon Url.Search (state.ActivePage = Url.Search) FA.FaSearch
                        ]
                    ]
                ]
            ]
        ]
    ]
