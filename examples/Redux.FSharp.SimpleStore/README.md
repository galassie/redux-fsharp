# Simple Store

### State:
``` fsharp
type State = { CurrentValue: int }
```

### Actions:
``` fsharp
type IncrementAction = { Amount: int }
type DecrementAction = { Amount: int }

type Actions =
    | Increment of IncrementAction
    | Decrement of DecrementAction
```

### Reducer:
``` fsharp
let incrementDecrementReducer state action =
    match action with
    | Increment { Amount = amount } -> { state with CurrentValue = state.CurrentValue + amount }
    | Decrement { Amount = amount } -> { state with CurrentValue = state.CurrentValue - amount }
```

### Subscriber:
``` fsharp
let consoleLogSubscriber state =
    printfn "Current value: %d" state.CurrentValue
```

### Program:
``` fsharp
[<EntryPoint>]
let main argv =
    let store = createStore incrementDecrementReducer { CurrentValue = 0 } id
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
```

### Output:
``` shell
Current value: 1
Current value: 3
Current value: 2
Expected Current value should be 1
Actual Current value is 1
```