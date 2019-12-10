open Redux.Types
open Redux.ApplyMiddleware
open Redux.Store

type State = { ToDoList: string list }

type AddNoteAction = { Note: string }

type Actions =
    | AddNote of AddNoteAction

let reducer state action =
    match action with
    | AddNote { Note = note } -> { state with ToDoList = state.ToDoList@[note] }

let logger (store : IStore<State, Actions>) next action =
    printfn "Will dispatch: %A" action
    let returnValue = next(action)
    printfn "State after dispatch: %A" <| store.GetState()
    returnValue

[<EntryPoint>]
let main argv =
    let store = createStore reducer { ToDoList = [] } (applyMiddleware [| logger |])

    store.Dispatch (AddNote { Note = "Learn F#" }) |> ignore
    store.Dispatch (AddNote { Note = "Learn Redux" }) |> ignore

    printfn "Last state: %A" <| store.GetState()
    0 // return an integer exit code
