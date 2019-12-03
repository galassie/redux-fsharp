open Redux.Enhancer
open Redux.Store

type State = { CurrentValue: int }

type IncrementAction = { Amount: int }
type DecrementAction = { Amount: int }

type Actions =
    | Increment of IncrementAction
    | Decrement of DecrementAction

let reducer state action =
    match action with
    | Increment { Amount = amount } -> { state with CurrentValue = state.CurrentValue + amount }
    | Decrement { Amount = amount } -> { state with CurrentValue = state.CurrentValue - amount }

let consoleLogSubscriber state =
    printfn "Current value: %d" state.CurrentValue

[<EntryPoint>]
let main argv =
    let store = createStore reducer { CurrentValue = 0 } IdStoreEnhancer
    let unsubscribe = store.Subscribe(consoleLogSubscriber)

    store.Dispatch (Increment { Amount = 1 }) |> ignore
    store.Dispatch (Increment { Amount = 2 }) |> ignore
    store.Dispatch (Decrement { Amount = 1 }) |> ignore

    unsubscribe() |> ignore

    store.Dispatch (Decrement { Amount = 1 }) |> ignore

    let lastState = store.GetState()
    printfn "Expected Current value should be 1"
    printfn "Actual Current value is %d" lastState.CurrentValue
    0 // return an integer exit code
