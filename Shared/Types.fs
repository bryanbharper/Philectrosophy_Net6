module Shared.Types

type Deferred<'T> =
    | Idle
    | InProgress
    | Resolved of 'T
    
