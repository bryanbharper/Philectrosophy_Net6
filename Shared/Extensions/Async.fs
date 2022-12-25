module Shared.Extensions.Async

let map mapping asyncOp =
    async.Bind(asyncOp, mapping >> async.Return)