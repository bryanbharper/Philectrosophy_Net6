namespace Shared.Extensions

open System

type MonthInt =
    private
    | MInt of int
    static member create n =
        n % 12
        |> Math.Abs
        |> fun i -> if i = 0 then 12 else i
        |> MInt
        
    member m.ToInt32() = let (MInt i) = m in i
    
type DayInt = 
    private
    | DInt of int
    static member create n =
        n % 31
        |> Math.Abs
        |> fun i -> if i = 0 then 31 else i
        |> DInt
        
    member d.ToInt32() = let (DInt i) = d in i
    
module Date =
    let monthName (m: MonthInt) =
        match m.ToInt32() with
        | 1 -> "January"
        | 2 -> "February"
        | 3 -> "March"
        | 4 -> "April"
        | 5 -> "May"
        | 6 -> "June"
        | 7 -> "July"
        | 8 -> "August"
        | 9 -> "September"
        | 10 -> "October"
        | 11 -> "November"
        | 12 -> "December"
        | _ -> failwith "Invalid month"
        
    let daySuffix (d: DayInt) =
        match d.ToInt32() with
        | 1 | 21 | 31 -> "st"
        | 2 | 22 -> "nd"
        | 3 | 23 -> "rd"
        | _ -> "th"
        
    let format (date: DateTime) =
        let month =
            date.Month |> MonthInt.create |> monthName

        let daySuffix = date.Day |> DayInt.create |> daySuffix

        $"%i{date.Day}%s{daySuffix} %s{month} %i{date.Year}"