using SampleMVVM.Wpf.Models.Entities;
using SampleMVVM.Wpf.Models.State.States;
using System;
using System.Collections.Immutable;

namespace SampleMVVM.Wpf.Models.State.Selectors
{
  public static class DataSelectors
  {
    public static IObservable<ImmutableList<DataItem>> Items
      = App.Store.Select<DataState, ImmutableList<DataItem>>(state => state.Items);
  }
}
