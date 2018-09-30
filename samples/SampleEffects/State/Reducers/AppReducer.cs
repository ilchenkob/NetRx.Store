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
