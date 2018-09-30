namespace SampleState.State.Actions
{
    public class SetEmail : NetRx.Store.Action<string>
    {
        public SetEmail(string payload) : base(payload)
        {
        }
    }
}
