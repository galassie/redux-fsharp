namespace Redux

open System.Collections.Generic

module Types =
    
    type IState =
        interface end

    type IAction = 
        abstract Type : string

    type Reducer<'S, 'A when 'S :> IState and 'A :> IAction> =
        'S -> 'A -> 'S

    type Unsubscribe =
        unit -> bool

    type IStoreSubscriber<'S when 'S :> IState> =
        abstract OnNewState : 'S -> unit

    type IStore<'S, 'A when 'S :> IState and 'A :> IAction> =
        abstract GetState : unit -> 'S
        abstract Dispatch : 'A -> 'A
        abstract Subscribe : IStoreSubscriber<'S> -> Unsubscribe
        abstract ReplaceReducer : Reducer<'S, 'A> -> unit