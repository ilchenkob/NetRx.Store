using SampleMVVM.Wpf.Models.State.States;
using System;
using System.Reactive.Linq;

namespace SampleMVVM.Wpf.Models.State.Selectors
{
  public static class RootSelectors
  {
    public static  IObservable<bool> IsLoading
      => IsUserLoading.CombineLatest(IsDataLoading, (x, y) => x || y);

    private static IObservable<bool> IsUserLoading = App.Store.Select<UserState, bool>(state => state.IsLoading);

    private static IObservable<bool> IsDataLoading = App.Store.Select<DataState, bool>(state => state.IsLoading);
  }
}
