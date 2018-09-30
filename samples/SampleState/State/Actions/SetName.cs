namespace SampleState.State.Actions
{
    public class SetName : NetRx.Store.Action<string>
    {
        public SetName(string payload) : base(payload)
        {
        }
    }
}
