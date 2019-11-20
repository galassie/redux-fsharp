module Redux.FSharp.Tests

open NUnit.Framework
open Redux.Store

[<Test>]
let CreateStoreShouldNotReturnNull () =
    let mockReducer state action = action
    let initialState = 0
    
    let store = createStore mockReducer initialState

    Assert.NotNull(store)

[<Test>]
let StoreGetStateShouldReturnProperState () =
    let mockReducer state action =
        match action with
        | "increment" -> state + 1
        | "decrement" -> state + 1
        | _ -> state
    let initialState = 0

    let store = createStore mockReducer initialState

    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore

    Assert.AreEqual(3, store.GetState())