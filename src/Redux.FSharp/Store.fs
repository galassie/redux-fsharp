namespace Redux

open System.Collections.Generic
open Redux.Types
open Redux.Enhancer

module Store =

    type Store<'S, 'A>(initialReducer : Reducer<'S, 'A>, initialState : 'S) =
        let mutable reducer = initialReducer
        let mutable state = initialState
        let mutable subscriptions = List<Subscriber<'S>>()

        interface IStore<'S, 'A> with
            member __.GetState() =
                state
            member __.Dispatch action =
                state <- reducer state action
                for sub in subscriptions do sub state
                action
            member __.Subscribe sub =
                subscriptions.Add(sub)
                fun () -> subscriptions.Remove(sub)
            member __.ReplaceReducer newReducer =
                reducer <- newReducer

    let createStore<'S, 'A> (reducer : Reducer<'S, 'A>) (initialState : 'S) (enhancer : StoreEnhancer<'S, 'A>) =
        enhancer <| new Store<'S, 'A>(reducer, initialState)