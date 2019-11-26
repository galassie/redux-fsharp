namespace Redux

open Redux.Types

module ApplyMiddleware =

    type Dispatch<'A> =
        'A -> 'A

    type Middleware<'S, 'A> =
        IStore<'S, 'A> -> Dispatch<'A> -> 'A -> 'A

    let applyMiddleware<'S, 'A> (middlewares: Middleware<'S, 'A> array) =
        let createEnhancedStore (store: IStore<'S, 'A>) = 
            { new IStore<'S, 'A> with
                    member __.GetState() = 
                        store.GetState()
                    member __.Dispatch action = 
                        let storeDispatch = store.Dispatch
                        let dispatch = 
                            middlewares
                            |> Array.rev
                            |> Array.fold (fun nextDispatch middleware -> middleware(store)(nextDispatch)) storeDispatch
                        dispatch(action)
                    member __.Subscribe sub =
                        store.Subscribe(sub)
                    member __.ReplaceReducer newReducer =
                        store.ReplaceReducer(newReducer)
            }
        createEnhancedStore