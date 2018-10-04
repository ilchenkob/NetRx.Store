using System.Collections.Immutable;
using NetRx.Store;
using actions = SampleState.State.Actions;

namespace SampleState.State.Reducers
{
    public struct Contact
    {
        public string Name { get; set; }

        public string Email { get; set; }
    }

    public struct ProfileState
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public ImmutableList<Contact> Contacts { get; set; }
    }

    public class ProfileReducer : Reducer<ProfileState>
    {
        public ProfileState Reduce(ProfileState state, actions.SetEmail action)
        {
            state.Email = action.Payload;
            return state;
        }

        public ProfileState Reduce(ProfileState state, actions.SetName action)
        {
            state.Name = action.Payload;
            return state;
        }

        public ProfileState Reduce(ProfileState state, actions.AddContact action)
        {
            state.Contacts = state.Contacts.Add(action.Payload);
            return state;
        }

        public ProfileState Reduce(ProfileState state, actions.ClearContacts action)
        {
            state.Contacts = state.Contacts.Clear();
            return state;
        }
    }
}
