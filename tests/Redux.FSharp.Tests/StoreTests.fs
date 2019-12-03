module Redux.FSharp.Tests.StoreTests

open NUnit.Framework
open Redux.Enhancer
open Redux.Store

[<Test>]
let CreateStoreShouldNotReturnNull () =
    let mockReducer state action = action
    let initialState = 0

    let store = createStore mockReducer initialState IdStoreEnhancer

    Assert.NotNull(store)

[<Test>]
let StoreGetStateShouldReturnProperState () =
    let mockReducer state action =
        match action with
        | "increment" -> state + 1
        | "decrement" -> state - 1
        | _ -> state
    let initialState = 0

    let store = createStore mockReducer initialState IdStoreEnhancer

    store.Dispatch "increment" |> ignore
    store.Dispatch "decrement" |> ignore
    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore

    Assert.AreEqual(2, store.GetState())

[<Test>]
let StoreDispatchShouldNotifyAllSubscribers () =
    let mutable subscriberState1 = 0
    let mutable subscriberState2 = 5
    let subscriber1 state =
        subscriberState1 <- state
    let subscriber2 state =
        subscriberState2 <- state

    let mockReducer state action =
        match action with
        | "increment" -> state + 1
        | "decrement" -> state - 1
        | _ -> state
    let initialState = 0

    let store = createStore mockReducer initialState IdStoreEnhancer

    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore

    store.Subscribe(subscriber1) |> ignore
    store.Subscribe(subscriber2) |> ignore

    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore
    store.Dispatch "decrement" |> ignore

    Assert.AreEqual(3, subscriberState1)
    Assert.AreEqual(3, subscriberState2)

[<Test>]
let StoreUnsubscribeShouldProperlyUnsubscribeSubscribers () =
    let mutable subscriberState1 = 0
    let mutable subscriberState2 = 5
    let subscriber1 state =
        subscriberState1 <- state
    let subscriber2 state =
        subscriberState2 <- state

    let mockReducer state action =
        match action with
        | "increment" -> state + 1
        | "decrement" -> state - 1
        | _ -> state
    let initialState = 0

    let store = createStore mockReducer initialState IdStoreEnhancer

    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore

    let unsubscribe1 = store.Subscribe(subscriber1)
    let unsubscribe2 = store.Subscribe(subscriber2)

    store.Dispatch "decrement" |> ignore

    let resultUnsubscribe1 = unsubscribe1()

    store.Dispatch "increment" |> ignore

    let resultUnsubscribe2 = unsubscribe2()

    store.Dispatch "increment" |> ignore

    Assert.IsTrue(resultUnsubscribe1)
    Assert.IsTrue(resultUnsubscribe2)
    Assert.AreEqual(1, subscriberState1)
    Assert.AreEqual(2, subscriberState2)

[<Test>]
let StoreReplaceReducerShouldProperlyUpdateTheReducer () =
    let mockReducer1 state action =
        match action with
        | "increment" -> state + 1
        | "decrement" -> state - 1
        | _ -> state
    let mockReducer2 state action =
        match action with
        | "increment" -> state * 3
        | "decrement" -> state - 2
        | _ -> state
    let initialState = 0

    let store = createStore mockReducer1 initialState IdStoreEnhancer

    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore
    store.Dispatch "increment" |> ignore
    store.Dispatch "decrement" |> ignore

    store.ReplaceReducer mockReducer2

    store.Dispatch "increment" |> ignore
    store.Dispatch "decrement" |> ignore

    Assert.AreEqual(4, store.GetState())