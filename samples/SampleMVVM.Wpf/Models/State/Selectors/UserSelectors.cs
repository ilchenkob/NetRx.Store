using SampleMVVM.Wpf.Models.State.States;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace SampleMVVM.Wpf.Models.State.Selectors
{
  public static class UserSelectors
  {
    public static IObservable<bool> IsLoggedIn
      = App.Store.Select<UserState, int>(state => state.UserId).Select(id => id > 0);

    public static IObservable<string> Login
      = App.Store.Select<UserState, string>(state => state.Login);

    public static IObservable<int> UserId
      = App.Store.Select<UserState, int>(state => state.UserId);
  }
}
