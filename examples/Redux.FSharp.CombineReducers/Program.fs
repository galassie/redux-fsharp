open Redux.CombineReducers
open Redux.Store

type State = {
    CounterValue: int;
    NoteList: (int * string) list
}

type AddNoteAction = { Id: int; Note: string }
type RemoveNoteAction = { Id: int }

type IncrementCounterAction = { Amount: int }
type DecrementCounterAction = { Amount: int }

type Actions =
    | Add of AddNoteAction
    | Remove of RemoveNoteAction
    | Increment of IncrementCounterAction
    | Decrement of DecrementCounterAction

let noteReducer state action =
    match action with
    | Add { Id = id; Note = note } -> { state with NoteList = (id, note)::state.NoteList }
    | Remove { Id = idToRemove } -> { state with NoteList = List.filter (fun (id, _) -> id <> idToRemove ) <| state.NoteList }
    | _ -> state

let counterReducer state action =
    match action with
    | Increment { Amount = amount } -> { state with CounterValue = state.CounterValue + amount }
    | Decrement { Amount = amount } -> { state with CounterValue = state.CounterValue - amount }
    | _ -> state

let mapNoteList = 
    List.map (fun (_, note) -> note)

let consoleLogSubscriber state =  
    printfn "Counter value: %d - Note list: %A" state.CounterValue (mapNoteList state.NoteList)

[<EntryPoint>]
let main argv =
    let reducer = combineReducers [| noteReducer; counterReducer |]
    let store = createStore reducer { CounterValue = 0; NoteList = [] } id
    let unsubscribe = store.Subscribe(consoleLogSubscriber)

    store.Dispatch (Increment { Amount = 5 }) |> ignore
    store.Dispatch (Add { Id = 1; Note = "Buy milk" }) |> ignore
    store.Dispatch (Add { Id = 2; Note = "Buy cake" }) |> ignore
    store.Dispatch (Decrement { Amount = 3 }) |> ignore
    store.Dispatch (Remove { Id = 1 }) |> ignore

    let lastState = store.GetState()
    printfn "Expected Counter value should be 2"
    printfn "Actual Counter value is %d" lastState.CounterValue
    printfn "Expected Note list should be [\"Buy cake\"]"
    printfn "Actual Note list is %A" <| mapNoteList lastState.NoteList
    0 // return an integer exit code
