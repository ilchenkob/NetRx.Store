using System;
using NetRx.Store;
using actions = SampleEffects.State.Actions;

namespace SampleEffects.State.Reducers
{
    public struct Data
    {
        public int Count { get; set; }

        public decimal Amount { get; set; }

        public string Category { get; set; }
    }

    public struct AppState
    {
        public bool IsLoading { get; set; }

        public string Username { get; set; }

        public Data Data { get; set; }

        public static AppState Initial => new AppState
        {
            IsLoading = false,
            Username = string.Empty,
            Data = new Data
            {
                Count = 0,
                Amount = 0,
                Category = string.Empty
            }
        };

        public static Func<AppState, NetRx.Store.Action, AppState> Reducer = (state, action) =>
        {
            if (action is actions.SetIsLoading setIsLoading)
            {
                state.IsLoading = setIsLoading.Payload;
                return state;
            }
            if (action is actions.SetUsername setUsername)
            {
                state.Username = setUsername.Payload;
                return state;
            }
            if (action is actions.LoadData)
            {
                state.IsLoading = true;
                return state;
            }
            if (action is actions.LoadDataSuccess loadDataSuccess)
            {
                state.IsLoading = false;
                state.Data = loadDataSuccess.Payload;
                return state;
            }
            return state;
        };
    }

    public class AppReducer : Reducer<AppState>
    {
        public AppState Reduce(AppState state, actions.SetIsLoading action)
        {
            state.IsLoading = action.Payload;
            return state;
        }

        public AppState Reduce(AppState state, actions.SetUsername action)
        {
            state.Username = action.Payload;
            return state;
        }

        public AppState Reduce(AppState state, actions.LoadData action)
        {
            state.IsLoading = true;
            return state;
        }

        public AppState Reduce(AppState state, actions.LoadDataSuccess action)
        {
            state.IsLoading = false;
            state.Data = action.Payload;
            return state;
        }
    }
}
