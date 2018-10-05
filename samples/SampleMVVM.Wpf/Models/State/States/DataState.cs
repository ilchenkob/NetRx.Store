using SampleMVVM.Wpf.Models.Entities;
using System.Collections.Immutable;

namespace SampleMVVM.Wpf.Models.State.States
{
  public struct DataState
  {
    public bool IsLoading { get; set; }

    public ImmutableList<DataItem> Items { get; set; }

    public static DataState Initial()
    {
      return new DataState
      {
        IsLoading = false,
        Items = ImmutableList.Create<DataItem>()
      };
    }
  }
}
