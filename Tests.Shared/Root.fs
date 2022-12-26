module Tests.Shared.Root


#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

open Shared
open Shared.Contracts

#if !FABLE_COMPILER
let routeProperty =
    testProperty "Route.builder properly formats route, any inputs."
    <| fun typeName methodName ->
        // arrange
        // act
        let result = Route.builder typeName methodName

        // assert
        result = $"/api/%s{typeName}/%s{methodName}"
#endif

let route =
    testCase "Route.builder properly formats route."
    <| fun _ ->
        // arrange
        let typeName = "type"
        let method = "method"

        // act
        let result = Route.builder typeName method

        // assert
        Expect.equal result $"/api/%s{typeName}/%s{method}" "Should be '/api/type/method.'"


let all = testList "|Shared Tests|" [
    Async.all
    Date.all
    Dtos.all
    Option.all
    Math.all
    route
    String.all
    Tuple.all

#if !FABLE_COMPILER
    routeProperty
#endif

]
