module Client.Pages.About

open Client.Styles
open Elmish
open Feliz

type State = unit

type Msg = unit

let init (): State * Cmd<Msg> = (), Cmd.none

let update (_: Msg) (_: State): State * Cmd<Msg> = (), Cmd.none

let section (bgColor: string) (children: ReactElement list) =
    Html.section [
        prop.classes [ Bulma.Section; Bulma.HasTextCentered; bgColor ]
        prop.children children
    ]

let sectionHeader (title: string) (subtitle: string) =
    [
        Html.h2 [
            prop.classes [ Bulma.Title; Bulma.HasTextWhite ]
            prop.text title
        ]
        Html.h4 [
            prop.classes [ Bulma.Subtitle; Bulma.HasTextWhite; Bulma.Mb5 ]
            prop.text subtitle
        ]
    ]
    |> Html.div

let sectionBlurb (blurb: string) =
    Html.div [
        prop.classes [ Bulma.Container ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.Content; Bulma.IsLarge; Bulma.Mb5 ]
                prop.children [
                    Html.p [
                        prop.classes [ Bulma.HasTextWhite ]
                        prop.text blurb
                    ]
                ]
            ]
        ]
    ]

let about =
    [
        Html.div [
            prop.classes [ Bulma.Image; Bulma.IsInlineBlock ]
            prop.children [
                Html.img [
                    prop.classes [ Bulma.IsRounded ]
                    prop.src "img/profile.jpg"
                ]
            ]
        ]
        Html.div [
            prop.classes [ Bulma.Columns ]
            prop.children [
                Html.div [
                    prop.classes [
                        Bulma.Column
                        Bulma.IsOffsetOneThird
                        Bulma.IsOneThird
                    ]
                    prop.children [
                        Html.div [
                            prop.classes [ Bulma.Content; Bulma.IsMedium ]
                            prop.children [
                                Html.p [
                                    prop.classes [
                                        Bulma.HasTextWhite
                                        Bulma.Mt3
                                    ]
                                    prop.text
                                        ("I'm Bryan. Philectrosophy is my blog."
                                         + " I've created it as a place to explore my interests."
                                         + " Hopefully other people will find it interesting too."
                                         + " Topics range from technical to personal.")
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
    |> section Style.DarkBg

let progressBar (color: string) (percent: float) (label: string) =
    Html.div [
        prop.className Style.ProgressWrapper
        prop.children [
            Html.progress [
                prop.classes [ Bulma.Progress; Bulma.IsMedium; color ]
                prop.max 100
                prop.value percent
                prop.text label
            ]
            Html.p [
                prop.classes [ Style.ProgressValue; Bulma.HasTextWhite ]
                prop.text label
            ]
        ]
    ]

let programming =
    let languages =
        [
            "C#"
            "F#"
            "Typescript / JavaScript"
            "GraphQL"
            "Haskell"
            "Python"
        ]

    let colors =
        [
            Bulma.IsPrimary
            Bulma.IsLink
            Bulma.IsInfo
            Bulma.IsSuccess
            Bulma.IsWarning
            Bulma.IsDanger
        ]

    let values =
        seq { 0.0 .. 0.15 .. 7.0 }
        |> Seq.map (fun x -> 2.0 ** x)
        |> Seq.filter ((>=) 100.0)
        |> Seq.rev
        |> Seq.take languages.Length
        |> Seq.map (round >> int)
        |> List.ofSeq

    [
        sectionHeader "Programming" "[ˑproʊgræmɪŋ] (noun)"

        sectionBlurb
            ("I work as a Software Engineer."
             + " Interested in functional programming, web development, testing, tooling, 2d games, visualizations, and more...")

        Html.div [
            prop.className Bulma.Columns
            prop.children [
                Html.div [
                    prop.className Bulma.Column
                    prop.children [
                        Html.div [
                            prop.className Bulma.Columns
                            prop.children [
                                Html.div [
                                    prop.classes [ Bulma.Column; Bulma.IsOffsetOneFifth; Bulma.IsThreeFifths ]
                                    
                                    List.zip3 colors values languages
                                    |> List.map (fun (c, v, l) -> progressBar c v l)
                                    |> prop.children
                                ]
                            ]
                        ]
                    ]
                ]
                
                Html.div [
                    prop.className Bulma.Column
                    prop.children [
                        Html.h4 [
                            prop.classes [ Bulma.Title; Bulma.HasTextWhite ]
                            prop.text "Follow me on:"
                        ]

                        Html.ul [
                            Html.li [
                                Html.a [
                                    prop.className Bulma.HasTextWhite
                                    prop.href "https://github.com/bryanbharper"
                                    prop.text "https://github.com/bryanbharper"
                                ]
                            ]
                            Html.li [
                                Html.a [
                                    prop.className Bulma.HasTextWhite
                                    prop.href "https://www.codewars.com/users/bryanbharper"
                                    prop.text "www.codewars.com/users/bryanbharper"
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
        
        Html.div [
            prop.classes [ Bulma.Columns ]
        ]

    ]
    |> section Style.DarkPurpleBg



let interestsAndProjects (interests: string list) (projects: (string * string) list) =
    Html.div [
        prop.classes [ Bulma.Columns ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.Column ]
                prop.children [
                    Html.h4 [
                        prop.classes [
                            Bulma.Title
                            Bulma.HasTextWhite
                            Bulma.Mb1
                        ]
                        prop.text "Areas of Interest:"
                    ]

                    interests
                    |> List.map (fun s ->
                        Html.li [
                            prop.className Bulma.HasTextWhite
                            prop.text s
                        ])
                    |> Html.ul
                ]
            ]

            Html.div [
                prop.classes [
                    Bulma.Column
                ]
                prop.children [
                    Html.h4 [
                        prop.classes [
                            Bulma.Title
                            Bulma.HasTextWhite
                            Bulma.Mb1
                        ]
                        prop.text "Related Posts / Projects"
                    ]
                    projects
                    |> List.map (fun (label, href) ->
                        Html.li [
                            Html.a [
                                prop.classes [
                                    Bulma.HasTextWhite
                                    Style.IsUnderlined
                                ]
                                prop.href href
                                prop.text label
                            ]
                        ])
                    |> Html.ul
                ]
            ]
        ]
    ]

let engineering =
    let interests =
        [
            "Circuits"
            "Digital Systems"
            "Control Systems"
            "Signal Theory"
        ]

    let projects =
        [
            "Digital Clock Design", "/blog/build-a-digital-clock"
            "555 Timer IC", "/blog/the-555-timer-ic"
            "Terminal Talk", "/blog/terminal-talk"
        ]

    [
        sectionHeader "Engineering" "[ˈɛnʤəˈnɪrɪŋ] (noun)"
        sectionBlurb
            "Earned a bachelor's of science in Electrical Engineering. I'm a bit rusty these days, but still tinker."
        interestsAndProjects interests projects
    ]
    |> section Style.PurpleBg

let philosophy =
    let interests =
        [
            "Metaphysics"
            "Logic"
            "Mind"
            "Language"
        ]

    let projects =
        [
            "Gunky Atoms", "/blog/gunky-atoms"
            "Identity of Indiscernibles", "/blog/the-identity-of-indiscernibles"
            "On Plantinga's E.A.A.N", "/blog/on-the-evolutionary-argument-against-naturalism"
        ]

    [
        sectionHeader "Philosophy" "[fəˈlɑsəfi] (noun)"
        sectionBlurb
            "Earned a master's degree in philosophy. Also rusty, but enjoy the occasional omphaloskepsis — yes, that's a fancy word for 'navel-gaze'."
        interestsAndProjects interests projects
    ]
    |> section Style.LightPurpleBg

let resumeBtn =
    Html.a [
        prop.classes [ Bulma.IsHiddenMobile ]
        prop.href "resources/Resume_BryanHarper.pdf"
        prop.target "_blank"
        prop.children [
            Html.div [
                prop.classes [ Style.ResumeTab ]
                prop.children [
                    Html.h4 "View Resume"
                    Html.span [
                        prop.classes [ FA.Fa; FA.FaFilePdf ]
                    ]
                ]
            ]
        ]
    ]

let render (_: State) (_: Msg -> unit): ReactElement =
    [
        resumeBtn
        about
        programming
        engineering
        philosophy
        section Style.LighterPurpleBg []
    ]
    |> Html.div
