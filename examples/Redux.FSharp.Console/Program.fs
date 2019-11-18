open Redux.Store
open Redux.Types

type IncrementAction = { Amount: int }
type DecrementAction = { Amount: int }

type Actions =
    | Increment of IncrementAction
    | Decrement of DecrementAction

type State = { CurrentValue: int}

let incrementDecrementReducer state action =
    match action with
    | Increment { Amount = x } -> { state with CurrentValue = state.CurrentValue + x }
    | Decrement { Amount = x } -> { state with CurrentValue = state.CurrentValue - x }

type ConsoleLogSubscriber() =
    interface IStoreSubscriber<State> with
        member this.OnNewState state =
            printfn "Current value: %d" state.CurrentValue

[<EntryPoint>]
let main argv =

    let store = createStore incrementDecrementReducer { CurrentValue = 0 }
    let sub = ConsoleLogSubscriber()
    let unsubscribe = store.Subscribe(sub)

    store.Dispatch (Increment { Amount = 1 }) |> ignore
    store.Dispatch (Increment { Amount = 2 }) |> ignore
    store.Dispatch (Decrement { Amount = 1 }) |> ignore

    unsubscribe() |> ignore

    store.Dispatch (Decrement { Amount = 1 }) |> ignore

    let lastState = store.GetState()
    printfn "Expected CurrentValue should be 1"
    printfn "Actual CurrentValue is %d" lastState.CurrentValue
    0 // return an integer exit code
