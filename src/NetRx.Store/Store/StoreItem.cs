namespace NetRx.Store
{
    internal sealed class StoreItem
    {
        internal StoreItem(StateWrapper state, dynamic reducer)
        {
            State = state;
            Reducer = reducer;
        }

        internal StateWrapper State { get; set; }

        internal dynamic Reducer { get; private set; }
    }
}
