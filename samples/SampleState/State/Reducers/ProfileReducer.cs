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

    public class ProfileReducerFunc
    {
        public ProfileState Reduce(ProfileState state, Action action)
        {
            if (action is actions.SetEmail setEmail)
            {
                state.Email = setEmail.Payload;
                return state;
            }
            if (action is actions.SetName setName)
            {
                state.Name = setName.Payload;
                return state;
            }
            if (action is actions.AddContact addContact)
            {
                state.Contacts = state.Contacts.Add(addContact.Payload);
                return state;
            }
            if (action is actions.ClearContacts)
            {
                state.Contacts = state.Contacts.Clear();
                return state;
            }

            return state;
        }
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
