using System;
using SampleEffects.State.Reducers;

namespace SampleEffects.State.Selectors
{
    public static class AppStateSelector
    {
        static AppStateSelector()
        {
            IsLoading = Store.Instance.Select<AppState, bool>(state => state.IsLoading);
            DataCategory = Store.Instance.Select<AppState, string>(state => state.Data.Category);
            DataAmount = Store.Instance.Select<AppState, decimal>(state => state.Data.Amount);
            Data = Store.Instance.Select<AppState, Data>(state => state.Data);
        }

        public static IObservable<bool> IsLoading { get; private set; }

        public static IObservable<string> DataCategory { get; private set; }

        public static IObservable<decimal> DataAmount { get; private set; }

        public static IObservable<Data> Data { get; private set; }
    }
}
