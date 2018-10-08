# NetRx.Store

State management for .Net projects, inspired by [@ngrx/store](https://github.com/ngrx/store)

[![NuGet](https://img.shields.io/nuget/v/NetRx.Store.svg?style=flat)](https://www.nuget.org/packages/NetRx.Store)


### Core concepts

Core principles are the same as in @ngrx/store:

* ***State*** is a single immutable data structure
* ***Actions*** represent state changes
* ***Reducers*** take the previous state and the next action to compute the new state
* ***Effects*** allows to express side effects through actions (typically asynchronous operations, like reading data from file, sending HTTP-request, etc.)
* ***Store*** holds states, their reducers and effects. It plays the role of *single source of truth*

### Overview

#### State

State cannot have reference type, it should be struct and can have properties of following types: simple type (```bool```, ```int```, ```double```, ```string```, etc), collection type from ```System.Collections.Immutable``` namespace or user defined ```struct```.

Example:

```csharp
public struct AppState
{
    public bool IsLoading { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public UserInfo User { get; set; }
    public ImmutableList<Person> Contacts { get; set; }
}
```

#### Action

User defined actions should be inherited from ```Action``` class defined in ```NetRx.Store``` namespace.

Example:

```csharp
public class RefreshStatus : NetRx.Store.Action
{
}

public class SetIsLoading : NetRx.Store.Action<bool>
{
    public SetIsLoading(bool payload) : base(payload)
    {
    }
}
```

#### Reducer

User defined reducers should be inherited from ```Reducer<TState>``` class defined in ```NetRx.Store``` namespace. Reducer should have a set of methods with the following syntax:

```public TSate Reduce(TState state, TAction action)```

Example:

```csharp
public class AppReducer : Reducer<AppState>
{
    public ProfileState Reduce(ProfileState state, SetIsLoading action)
    {
        state.IsLoading = action.Payload;
        return state;
    }

    public ProfileState Reduce(ProfileState state, SetUserName action)
    {
        state.User.Name = action.Payload;
        return state;
    }

    public ProfileState Reduce(ProfileState state, AddContact action)
    {
        state.Contacts = state.Contacts.Add(action.Payload);
        return state;
    }

    public ProfileState Reduce(ProfileState state, ClearContacts action)
    {
        state.Contacts = state.Contacts.Clear();
        return state;
    }
}
```

#### Effects

User defined effects should be inherited from ```Effect``` class defined in ```NetRx.Effects``` namespace.

Example:

```csharp
public class UsernameChangedEffect : Effect<SetUsername>
{
    public override async Task Invoke(SetUsername action)
    {
        await NotificationService.NotifyUserNameChange(action.Payload);
    }
}

public class LoadDataEffect : Effect<LoadData, LoadDataSuccess>
{
    public override async Task<LoadDataSuccess> Invoke(LoadData action)
    {
        var result = await DataService.Load();
        return new LoadDataSuccess(new Data
        {
            Count = result.Count,
            Category = result.Category,
            Timestamp = DateTime.UtcNow
        });
    }
}
```

#### Store

Store can be created by calling static method ```Store.Create()```. It returns the instance of ```BlankStore``` - store that doesn't have any states, reducers or effects. To put some state and reducer into it you have to call ```WithState``` method and pass the initial state and the reducer to it.

Example:

```csharp
using NetRx.Store

...

var initialAppState = new AppState
{
    IsLoading = false,
    Status = "none" 
};

var store = Store.Create()
     .WithState(initialAppState, new AppReducer())
     .WithState(new ContactState(), new ContactReducer())
     .WithState(new ProductState(), new ProductReducer());
```

As you can see from the example above, you can call WithState method few times one by one and you will get the store that contains all of the passed states with their reducers. After that you can subscribe to listen to the state properties changes:

```csharp
store.Select<AppState, string>(state => state.Status)
     .Subscribe(value =>
     {
	     Console.WriteLine(value);
     })
```

To trigger state changes you can call ```Dispatch``` method on the store:

```csharp
store.Dispatch(new SetIsLoading(true));
store.Dispatch(new RefreshStatus());
```

To register some effects in your store, you can call ```WithEffects``` method right after adding states and reducers to it:

```csharp
Store.Create()
     .WithState(initialAppState, new AppReducer())
     .WithEffects(new Effect[] { new LoadDataEffect(), new UsernameChangedEffect() });
```

In this case passed effects will be invoked when the corresponding action will be dispatched in the store.

### Examples
You can find ***sample projects*** [here](https://github.com/ilchenkob/NetRx.Store/tree/master/samples)