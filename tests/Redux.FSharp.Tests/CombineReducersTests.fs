module Redux.FSharp.Tests.CombineReducersTests

open NUnit.Framework
open Redux.Enhancer
open Redux.CombineReducers
open Redux.Store


[<Test>]
let CombineReducersShouldCombineProperly () =
    let initialState = {|
        Counter = 0;
        Text = "";
        List = []
    |}
    let reducer1 (state: {| Counter: int; Text: string; List: int list |}) action =
        match action with
        | "incCount" -> {| state with Counter = state.Counter + 1 |}
        | "decCount" -> {| state with Counter = state.Counter - 1 |}
        | _ -> state
    let reducer2 (state: {| Counter: int; Text: string; List: int list |}) action =
        match action with
        | "appendTxtHello" -> {| state with Text = state.Text + "Hello" |}
        | "appendTxtWorld" -> {| state with Text = state.Text + "World" |}
        | _ -> state
    let reducer3 (state: {| Counter: int; Text: string; List: int list |}) action =
        match action with
        | "appendInt" -> {| state with List = state.List@[1] |}
        | _ -> state


    let finalReducer = combineReducers [|
        reducer1
        reducer2
        reducer3
    |]
    let store = createStore finalReducer initialState id

    store.Dispatch "incCount" |> ignore
    store.Dispatch "incCount" |> ignore
    store.Dispatch "decCount" |> ignore
    store.Dispatch "appendTxtHello" |> ignore
    store.Dispatch "appendTxtWorld" |> ignore
    store.Dispatch "appendTxtHello" |> ignore
    store.Dispatch "appendTxtWorld" |> ignore
    store.Dispatch "appendTxtHello" |> ignore
    store.Dispatch "appendTxtHello" |> ignore
    store.Dispatch "appendTxtWorld" |> ignore
    store.Dispatch "appendInt" |> ignore
    store.Dispatch "appendInt" |> ignore
    store.Dispatch "appendInt" |> ignore

    let finalState = store.GetState()
    Assert.AreEqual(1, finalState.Counter)
    Assert.AreEqual("HelloWorldHelloWorldHelloHelloWorld", finalState.Text)
    Assert.AreEqual([1; 1; 1], finalState.List)
