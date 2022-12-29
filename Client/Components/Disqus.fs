﻿module Client.Components.Disqus

open Fable.Core
open Fable.Core.JsInterop
open Fable.React.Helpers
open Fable.React

type DisqusConfig =
    {
        url: string
        identifier: string
        title: string
    }

type DisqusProps =
    | [<CompiledName("shortname")>] Shortname of string
    | [<CompiledName("config")>] Config of DisqusConfig
    interface IHTMLProp

let inline render slug title =
    let props = [
                Shortname "philectrosophy"
                Config {
                    url = "http://philectrosophy.com/blog/" + slug
                    identifier = slug
                    title = title
                }
            ]

    ofImport "DiscussionEmbed" "disqus-react" (keyValueList CaseRules.LowerFirst props) []
