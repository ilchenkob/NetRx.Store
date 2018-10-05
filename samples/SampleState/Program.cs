using System;
using SampleState.State.Reducers;
using NetRx.Store;
using actions = SampleState.State.Actions;
using System.Collections.Immutable;

namespace SampleState
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialState = new ProfileState
            {
                Email = string.Empty,
                Name = string.Empty,
                Contacts = ImmutableList.Create<Contact>()
            };

            var store = Store.Create().WithState(initialState, new ProfileReducer());

            store.Select<ProfileState, string>(state => state.Name).Subscribe(value =>
            {
                Console.WriteLine($"Name: {value}");
            });
            store.Select<ProfileState, string>(state => state.Email).Subscribe(value =>
            {
                Console.WriteLine($"Email: {value}");
            });
            store.Select<ProfileState, ImmutableList<Contact>>(state => state.Contacts).Subscribe(value =>
            {
                Console.WriteLine($"Contacts: {value.Count}");
            });
            store.Select<ProfileState, ProfileState>(state => state).Subscribe(value =>
            {
                Console.WriteLine($"State value: {value.Name} {value.Email} {value.Contacts.Count}");
            });

            store.Dispatch(new actions.SetEmail("test@mail.com"));
            store.Dispatch(new actions.SetName("Test Profile"));
            store.Dispatch(new actions.AddContact(new Contact { Name = "Contact 1", Email = "contact_1@mail.com" }));
            store.Dispatch(new actions.AddContact(new Contact { Name = "Contact 2", Email = "contact_2@mail.com" }));
            store.Dispatch(new actions.AddContact(new Contact { Name = "Contact 3", Email = "contact_3@mail.com" }));
            store.Dispatch(new actions.ClearContacts());

            Console.ReadLine();
        }
    }
}
