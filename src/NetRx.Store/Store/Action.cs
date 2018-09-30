namespace NetRx.Store
{
    public abstract class Action
    {
    }

    public abstract class Action<TPayload> : Action
    {
        public TPayload Payload { get; private set; }

        public Action(TPayload payload) => Payload = payload;
    }
}
