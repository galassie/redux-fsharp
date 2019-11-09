namespace Redux

open System.Collections.Generic
open Redux.Types

module Store =
    
    type Store<'S, 'A when 'S :> IState and 'A :> IAction>(initialReducer: Reducer<'S, 'A>, initialState: 'S) =
        let mutable reducer = initialReducer
        let mutable state = initialState 
        let mutable subscribers = List<IStoreSubscriber<'S>>()

        interface IStore<'S, 'A> with
            member this.GetState() =
                state
            member this.Dispatch action =
                state <- reducer state action
                for sub in subscribers do sub.OnNewState state
                action
            member this.Subscribe ss =
                subscribers.Add(ss)
                fun () -> subscribers.Remove(ss)
            member this.ReplaceReducer newReducer =
                reducer <- newReducer

    let createStore<'S, 'A when 'S :> IState and 'A :> IAction> reducer initialState =
        new Store<'S, 'A>(reducer, initialState)