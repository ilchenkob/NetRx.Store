using SampleEffects.State.Reducers;
using SampleEffects.State.Effects;
using NetRxStore = NetRx.Store.Store;
using NetRx.Store.Effects;

namespace SampleEffects.State
{
    public static class Store
    {
        public static NetRxStore Instance { get; private set; }

        static Store()
        {
            Instance = NetRxStore.Create()
                                 .WithState(AppState.Initial, AppState.Reducer)
                                 // or
                                 // .WithState(AppState.Initial, new AppReducer())
                                 .WithEffects(new Effect[] { new LoadDataEffect(), new UsernameChangedEffect() });
        }
    }
}
