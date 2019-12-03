namespace Redux

open Redux.Types

module ApplyMiddleware =

    type Dispatch<'A> =
        'A -> 'A

    type Middleware<'S, 'A> =
        IStore<'S, 'A> -> Dispatch<'A> -> 'A -> 'A

    let applyMiddleware<'S, 'A> (middlewares : Middleware<'S, 'A> array) (store : IStore<'S, 'A>) =
        let innerDispatch =
            middlewares
            |> Array.rev
            |> Array.fold (fun nextDispatch middleware -> middleware store nextDispatch) store.Dispatch

        {
            new IStore<'S, 'A> with
                member __.GetState() =
                    store.GetState()
                member __.Dispatch action =
                    innerDispatch(action)
                member __.Subscribe sub =
                    store.Subscribe(sub)
                member __.ReplaceReducer newReducer =
                    store.ReplaceReducer(newReducer)
        }