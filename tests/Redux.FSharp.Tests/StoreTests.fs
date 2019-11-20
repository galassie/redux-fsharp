module Redux.FSharp.Tests

open NUnit.Framework
open Redux.Store

[<Test>]
let CreateStore_ShouldNotReturnNull () =
    let mockReducer state action = action
    let initialState = 0
    
    let store = createStore mockReducer initialState

    Assert.NotNull(store)