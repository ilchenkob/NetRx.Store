using SampleState.State.Reducers;

namespace SampleState.State.Actions
{
    public class AddContact : NetRx.Store.Action<Contact>
    {
        public AddContact(Contact payload) : base(payload)
        {
        }
    }
}
