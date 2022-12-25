module Tests.Shared.Date


#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open System
open FsCheck

open Shared.Extensions

let months =
    [
        "January"
        "February"
        "March"
        "April"
        "May"
        "June"
        "July"
        "August"
        "September"
        "October"
        "November"
        "December"
    ]

#if !FABLE_COMPILER
let properties = testList "|Date Property Tests|" [

    testProperty "MonthInt.create/ToInt32: value is always between 1 and 12."
    <| fun (randVal: int) ->
        // act
        let result = MonthInt.create randVal

        // arrange
        let i = result.ToInt32()

        // assert
        Expect.isGreaterThanOrEqual i 1 "MonthInt value is greater than or equal to 1."
        Expect.isLessThanOrEqual i 12 "MonthInt value is less than or equal to 12."

    testProperty "MonthInt.create/ToInt32: mod 12"
    <| fun (PositiveInt (randVal: int)) ->
        // act
        let result = MonthInt.create randVal

        // arrange
        let i = result.ToInt32()

        // assert
        if randVal % 12 = 0
        then Expect.equal i 12 //i = 12
        else Expect.equal i (randVal % 12)

    testProperty "DayInt.create/ToInt32: value is always between 1 and 31."
    <| fun (randVal: int) ->
        // act
        let result = DayInt.create randVal

        // arrange
        let i = result.ToInt32()

        // assert
        Expect.isGreaterThanOrEqual i 1 "DayInt value is greater than or equal to 1."
        Expect.isLessThanOrEqual i 31 "DayInt value is less than or equal to 31."

    testProperty "MonthInt.create/ToInt32: mod 31"
    <| fun (PositiveInt (randVal: int)) ->
        // act
        let result = DayInt.create randVal

        // arrange
        let i = result.ToInt32()

        // assert
        if randVal % 31 = 0
        then Expect.equal i 31
        else Expect.equal i (randVal % 31)

    testProperty "Date.monthName: maps to correct name."
    <| fun (PositiveInt (randVal: int)) ->
        // arrange
        let mInt = MonthInt.create randVal

        // act
        let name = Date.monthName mInt

        // assert
        Expect.equal name months[mInt.ToInt32() - 1] "Month name should match"

]
#endif
let all = testList "|Date|" [

#if !FABLE_COMPILER
    properties
#endif

    testCase "Date.format correctly formats the date"
    <| fun _ ->
        // arrange
        let date = DateTime(1969, 07, 20)

        // act
        let result = Date.format date

        // assert
        Expect.equal result "20th July 1969" "Houston, we have a problem."

    testCase "Date.daySuffix: handles 'st' cases"
    <| fun _ ->
        for d in [1; 21; 31] do
            // arrange
            let expected = "st"

            // act
            let result = d |> DayInt.create |> Date.daySuffix

            // assert
            Expect.equal result expected $"Wrong suffix for %i{d}"

    testCase "Date.daySuffix: handles 'nd' cases"
    <| fun _ ->
        for d in [2; 22] do
            // arrange
            let expected = "nd"

            // act
            let result = d |> DayInt.create |> Date.daySuffix

            // assert
            Expect.equal result expected $"Wrong suffix for %i{d}"

    testCase "Date.daySuffix: handles 'rd' cases"
    <| fun _ ->
        for d in [3; 23] do
            // arrange
            let expected = "rd"

            // act
            let result = d |> DayInt.create |> Date.daySuffix

            // assert
            Expect.equal result expected $"Wrong suffix for %i{d}"

    testCase "Date.daySuffix: handles 'default' cases"
    <| fun _ ->
        let testData =
            [1..31]
            |> List.filter (fun d -> not <| List.contains d [1; 21; 31; 2; 22; 3; 23 ])

        for d in testData do
            // arrange
            let expected = "th"

            // act
            let result = d |> DayInt.create |> Date.daySuffix

            // assert
            Expect.equal result expected $"Wrong suffix for %i{d}"

]
