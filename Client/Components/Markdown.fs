namespace Client.Components

open Feliz
open Fable.Core
open Fable.Core.JsInterop

[<Erase>]
type markdown =
    static member inline children (markdownString: string) = Interop.mkAttr "children" markdownString
    static member inline className (className: string) = Interop.mkAttr "className" className
    static member inline skipHtml (skipHtml: bool) = Interop.mkAttr "skipHtml" skipHtml
    static member inline linkTarget (linkTarget: string) = Interop.mkAttr "linkTarget" linkTarget
    static member inline rehypePlugins (rehypePlugins: obj array) = Interop.mkAttr "rehypePlugins" rehypePlugins

type Markdown =
    static member inline render (props: IReactProperty list) =
        Interop.reactApi.createElement (importDefault "react-markdown", createObj !!props)

module Rehype =
    [<ImportDefault("rehype-raw")>]
    let raw: obj = jsNative
