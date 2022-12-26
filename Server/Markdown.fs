module Server.Markdown

open System.Text.RegularExpressions
open Shared.Extensions

let codeCogsUrl = "http://latex.codecogs.com/gif.latex?"
let patternTemplate keyword =
    $"\[%s{keyword}\](.*?)\[\/%s{keyword}\]"

let displayMathKeyword = "MATH"
let displayMathOpenTag = $"[%s{displayMathKeyword}]"
let displayMathCloseTag = $"[/%s{displayMathKeyword}]"
let displayMathPattern = patternTemplate displayMathKeyword


let inlineMathKeyword = "IMATH"
let inlineMathOpenTag = $"[%s{inlineMathKeyword}]"
let inlineMathCloseTag = $"[/%s{inlineMathKeyword}]"
let inlineMathPattern = patternTemplate inlineMathKeyword
let codeCogsUrlInline = codeCogsUrl + "%5Cinline%20%5Csmall%20"

let popoverKeyword = "POP"
let popoverOpenTag = $"[%s{popoverKeyword}]"
let popoverCloseTag = $"[/%s{popoverKeyword}]"
let popoverPattern = patternTemplate popoverKeyword

let image url = $"![equation](%s{url})"

let convert pattern converter input =
    Regex.Matches(input, pattern)
    |> Seq.map (fun m -> m.Value, m.Groups[1].Value |> converter)
    |> Seq.fold (fun acc (m, v) -> acc |> String.replace m v) input

module Latex =
    let encodedImage prefix input =
        input |> String.urlEncode |> (+) prefix |> image

    let displayMathImage = encodedImage codeCogsUrl

    let inlineMathImage = encodedImage codeCogsUrlInline

    let convertDisplayMath = convert displayMathPattern displayMathImage

    let convertInlineMath = convert inlineMathPattern inlineMathImage

    let convertMath = convertDisplayMath >> convertInlineMath

module Bulma =
    let wrapInPopover input =
        """<sup class="popover"><span class="icon is-small"><i class="fas fa-window-restore"></i></span><span class="popover-content">"""
        + input
        + "</span></sup>"

    let convertPopovers = convert popoverPattern wrapInPopover
