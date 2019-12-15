# redux-fsharp

[![Build status](https://ci.appveyor.com/api/projects/status/89m308sqdtd2uyrk?svg=true)](https://ci.appveyor.com/project/galassie/redux-fsharp) [![Build Status](https://travis-ci.org/galassie/redux-fsharp.svg?branch=master)](https://travis-ci.org/galassie/redux-fsharp) [![NuGet](https://img.shields.io/nuget/v/redux-fsharp.svg)](https://nuget.org/packages/redux-fsharp)

Predictable state container for F# applications.

redux-fsharp is a [Redux](https://github.com/reduxjs/redux)-like implementation of the unidirectional data flow architecture in F#.

## Usage

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

## Examples

- [Simple Store](https://github.com/galassie/redux-fsharp/tree/master/examples/Redux.FSharp.SimpleStore)
- [Combine Reducers](https://github.com/galassie/redux-fsharp/tree/master/examples/Redux.FSharp.CombineReducers)
- [Apply Middleware](https://github.com/galassie/redux-fsharp/tree/master/examples/Redux.FSharp.ApplyMiddleware)

## Contributing

Code contributions are  more than welcome! 😻

Please commit any pull requests against the `master` branch.  
If you find any issue, please [report it](https://github.com/galassie/redux-fsharp/issues)!

## License

This project is licensed under [The MIT License (MIT)](https://raw.githubusercontent.com/galassie/redux-fsharp/master/LICENSE.md).

Author: [Enrico Galassi](https://twitter.com/enricogalassi88)
