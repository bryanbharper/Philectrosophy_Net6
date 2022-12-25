module Shared.Extensions.String

open System

let contains (s: string) (sub: string) = s.Contains sub
let trim (s: string) = s.Trim()
let ofChars (chars: char []) = String chars
let split (splitter: char) (s: string) = s.Split splitter
let join (joiner: string) (s: string seq) = String.Join(joiner, s)
let replace (oldVal: string) (newVal: string) (s: string) = s.Replace(oldVal, newVal)

let strip (stripChars: string) (str: string): string =
    let removeChar (str: string) (char': char) = str.Replace(char' |> string, "")

    Seq.fold removeChar str stripChars
    
let toLower (s: string) = s.ToLower()
let isNullOrWhiteSpace (s: string) = String.IsNullOrWhiteSpace s
let suffix (suffix': string) (s: string) = s + suffix'

let slugify =
    toLower
    >> trim
    >> strip "!\"#$%&'()*+,./:;?@[\]^_`{|}~"
    >> replace " " "-"
    >> replace "--" "-"
    
let urlEncode (s: string) = Uri.EscapeDataString s