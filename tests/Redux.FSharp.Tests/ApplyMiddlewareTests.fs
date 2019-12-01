module Redux.FSharp.Tests.ApplyMiddlewareTests

open NUnit.Framework
open Redux.Types
open Redux.ApplyMiddleware

[<Test>]
let ApplyMiddlewareWithoutMiddlewaresShouldCallBasicStoreFunctions () =
    let mutable isGetStateInvoked = false
    let mutable isDispatchInvoked = false
    let mutable isSubscribeInvoked = false
    let mutable isReplaceReducerInvoked = false

    let mockStore =
        {
            new IStore<int, string> with
                member __.GetState() =
                    isGetStateInvoked <- true
                    0
                member __.Dispatch action =
                    isDispatchInvoked <- true
                    action
                member __.Subscribe sub =
                    isSubscribeInvoked <- true
                    fun () -> true
                member __.ReplaceReducer newReducer =
                    isReplaceReducerInvoked <- true
        }
    let storeEnhanced = applyMiddleware [||] mockStore

    storeEnhanced.GetState() |> ignore
    Assert.IsTrue(isGetStateInvoked)

    storeEnhanced.Dispatch "test" |> ignore
    Assert.IsTrue(isDispatchInvoked)

    storeEnhanced.Subscribe(ignore) |> ignore
    Assert.IsTrue(isSubscribeInvoked)

    let mockReducer state action =
        state
    storeEnhanced.ReplaceReducer(mockReducer)
    Assert.IsTrue(isReplaceReducerInvoked)

[<Test>]
let ApplyMiddlewareWithMiddlewaresShouldInvokeDispatchInproperOrder () =
    let createMockMiddleware index store next action =
        let nextAction = action + sprintf " -> middleware n.%d" index
        next(nextAction)
    let mockMiddleware1 = createMockMiddleware 1
    let mockMiddleware2 = createMockMiddleware 2
    let mockMiddleware3 = createMockMiddleware 3

    let mockStore =
        {
            new IStore<string, string> with
                member __.GetState() =
                    ""
                member __.Dispatch action =
                    action + " -> store"
                member __.Subscribe sub =
                    fun () -> true
                member __.ReplaceReducer newReducer = ()
        }
    let storeEnhanced = applyMiddleware [| mockMiddleware1; mockMiddleware2; mockMiddleware3 |] mockStore

    let resultCallSequence = storeEnhanced.Dispatch "test"

    Assert.AreEqual("test -> middleware n.1 -> middleware n.2 -> middleware n.3 -> store", resultCallSequence)


[<Test>]
let ApplyMiddlewareWithMiddlewaresShouldAbleToGetStateProperly () =
    let mutable innerMutableState = 10
    let createMockMiddleware index (store : IStore<string, string>) next action =
        let state = store.GetState()
        let nextAction = action + sprintf " -> middleware -state %s- n.%d" state index
        innerMutableState <- innerMutableState + 1
        next(nextAction)
    let mockMiddleware1 = createMockMiddleware 1
    let mockMiddleware2 = createMockMiddleware 2
    let mockMiddleware3 = createMockMiddleware 3

    let mockStore =
        {
            new IStore<string, string> with
                member __.GetState() =
                    sprintf "%d" innerMutableState
                member __.Dispatch action =
                    action + " -> store"
                member __.Subscribe sub =
                    fun () -> true
                member __.ReplaceReducer newReducer = ()
        }
    let storeEnhanced = applyMiddleware [| mockMiddleware1; mockMiddleware2; mockMiddleware3 |] mockStore

    let resultCallSequence = storeEnhanced.Dispatch "test"

    Assert.AreEqual("test -> middleware -state 10- n.1 -> middleware -state 11- n.2 -> middleware -state 12- n.3 -> store", resultCallSequence)


[<Test>]
let ApplyMiddlewareWithMiddlewaresDispatchTheStoreShouldSkipOtherMiddlewares () =
    let createMockMiddleware index store next action =
        let nextAction = action + sprintf " -> middleware n.%d" index
        next(nextAction)
    let mockMiddleware1 (store : IStore<string, string>) next action = 
        let nextAction = action + " -> middleware skip to store"
        store.Dispatch nextAction
    let mockMiddleware2 = createMockMiddleware 2
    let mockMiddleware3 = createMockMiddleware 3

    let mockStore =
        {
            new IStore<string, string> with
                member __.GetState() =
                    ""
                member __.Dispatch action =
                    action + " -> store"
                member __.Subscribe sub =
                    fun () -> true
                member __.ReplaceReducer newReducer = ()
        }
    let storeEnhanced = applyMiddleware [| mockMiddleware1; mockMiddleware2; mockMiddleware3 |] mockStore

    let resultCallSequence = storeEnhanced.Dispatch "test"

    Assert.AreEqual("test -> middleware skip to store -> store", resultCallSequence)