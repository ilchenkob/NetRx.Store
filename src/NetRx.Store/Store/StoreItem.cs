namespace NetRx.Store
{
    internal sealed class StoreItem
    {
        internal StoreItem(StateWrapper state, ReducerWrapper reducer)
        {
            State = state;
            Reducer = reducer;
        }

        internal StateWrapper State { get; set; }

        internal ReducerWrapper Reducer { get; private set; }
  }
}
