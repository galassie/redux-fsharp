open Redux.Types
open Redux.Store
open Redux.ApplyMiddleware

type State = { ToDoList: string list }

type AddNoteAction = { Note: string }

type Actions =
    | AddNote of AddNoteAction

let reducer state action =
    match action with
    | AddNote { Note = note } -> { state with ToDoList = state.ToDoList@[note] }

let logger (store : IStore<State, Actions>) next action =
    printfn "Will dispatch: %A" <| action
    let returnValue = next(action)
    printfn "State after dispatch: %A" <| store.GetState()
    returnValue 

[<EntryPoint>]
let main argv =
    let store = createStore reducer { ToDoList = [] } 
    let storeEnhanced = applyMiddleware [| logger |] store

    storeEnhanced.Dispatch (AddNote { Note = "Learn F#" }) |> ignore
    storeEnhanced.Dispatch (AddNote { Note = "Learn Redux" }) |> ignore

    printfn "Last state: %A" <| storeEnhanced.GetState()
    0 // return an integer exit code
