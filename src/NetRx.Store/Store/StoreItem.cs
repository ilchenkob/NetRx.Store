using System;

namespace NetRx.Store
{
    internal class StoreItem
    {
        protected StoreItem(StateWrapper state)
        {
            State = state;
        }

        internal StoreItem(StateWrapper state, ReducerWrapper reducer) : this(state)
        {
            Reducer = reducer;
        }

        internal StateWrapper State { get; set; }

        internal ReducerWrapper Reducer { get; private set; }

        internal virtual object Dispatch<T>(T action, string actionTypeName) where T : Action
        {
            return Reducer.CanHandle(actionTypeName)
                    ? Reducer.Invoke(actionTypeName, State.Original, action)
                    : null;
        }
    }

    internal class StoreItem<TState> : StoreItem
    {
        internal StoreItem(StateWrapper state, Func<TState, Action, TState> reducerFunc) : base(state)
        {
            ReducerFunc = reducerFunc;
        }

        internal Func<TState, Action, TState> ReducerFunc { get; private set; }

        internal override object Dispatch<T>(T action, string actionTypeName)
        {
            return ReducerFunc((TState)State.Original, action);
        }
    }
}
