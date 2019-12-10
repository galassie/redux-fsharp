namespace Redux

open Redux.Types

module Enhancer =

    type StoreEnhancer<'S, 'A> =
        IStore<'S, 'A> -> IStore<'S, 'A>