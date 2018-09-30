namespace SampleEffects.State.Actions
{
    public class SetUsername : NetRx.Store.Action<string>
    {
        public SetUsername(string payload) : base(payload)
        {
        }
    }
}
