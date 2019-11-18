namespace Redux

open Redux.Types

module CombineReducers =

    let combineReducers (reducers: Reducer<'S, 'A> array) =
        fun state action ->
            Array.fold (fun state reducer -> reducer state action) state reducers