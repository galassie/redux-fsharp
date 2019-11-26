namespace Redux

module Types =

    type Reducer<'S, 'A> =
        'S -> 'A -> 'S

    type Unsubscribe =
        unit -> bool

    type Subscriber<'S> =
        'S -> unit

    type IStore<'S, 'A> =
        abstract GetState : unit -> 'S
        abstract Dispatch : 'A -> 'A
        abstract Subscribe : Subscriber<'S> -> Unsubscribe
        abstract ReplaceReducer : Reducer<'S, 'A> -> unit