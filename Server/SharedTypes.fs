﻿module Server.SharedTypes

open System
open System.Linq.Expressions

type FunAs() =
    static member LinqExpression<'T, 'TResult>(e: Expression<Func<'T, 'TResult>>) = e