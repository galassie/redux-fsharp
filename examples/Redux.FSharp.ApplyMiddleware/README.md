## Apply Middleware

State:
``` fsharp
type State = { ToDoList: string list }  
```

Actions:
``` fsharp
type AddNoteAction = { Note: string }

type Actions =
    | AddNote of AddNoteAction
```

Reducers:
``` fsharp
let reducer state action =
    match action with
    | AddNote { Note = note } -> { state with ToDoList = state.ToDoList@[note] }
```

Middleware:
``` fsharp
let logger (store : IStore<State, Actions>) next action =
    printfn "Will dispatch: %A" action
    let returnValue = next(action)
    printfn "State after dispatch: %A" <| store.GetState()
    returnValue
```

Program:
``` fsharp
[<EntryPoint>]
let main argv =
    let store = createStore reducer { ToDoList = [] } (applyMiddleware [| logger |])

    store.Dispatch (AddNote { Note = "Learn F#" }) |> ignore
    store.Dispatch (AddNote { Note = "Learn Redux" }) |> ignore

    printfn "Last state: %A" <| store.GetState()
    0 // return an integer exit code
```

Output:
``` shell
Will dispatch: AddNote { Note = "Learn F#" }
State after dispatch: { ToDoList = ["Learn F#"] }
Will dispatch: AddNote { Note = "Learn Redux" }
State after dispatch: { ToDoList = ["Learn F#"; "Learn Redux"] }
Last state: { ToDoList = ["Learn F#"; "Learn Redux"] }
```