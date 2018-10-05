using NetRx.Store;
using SampleMVVM.Wpf.Models.State.States;
using System.Collections.Immutable;
using System.Linq;

namespace SampleMVVM.Wpf.Models.State.Reducers
{
  public class DataStateReducer : Reducer<DataState>
  {
    public DataState Reduce(DataState state, DataActions.LoadDataStart action)
    {
      state.IsLoading = true;
      return state;
    }

    public DataState Reduce(DataState state, DataActions.LoadDataSuccess action)
    {
      state.Items = action.Payload.ToImmutableList();
      state.IsLoading = false;
      return state;
    }

    public DataState Reduce(DataState state, DataActions.LoadDataFailed action)
    {
      state.IsLoading = false;
      return state;
    }

    public DataState Reduce(DataState state, DataActions.SendItemStart action)
    {
      state.IsLoading = true;
      return state;
    }

    public DataState Reduce(DataState state, DataActions.SendItemSuccess action)
    {
      state.Items = state.Items.Remove(state.Items.First(d => d.Id == action.Payload.Id));
      state.IsLoading = false;
      return state;
    }

    public DataState Reduce(DataState state, DataActions.SendItemFailed action)
    {
      state.IsLoading = false;
      return state;
    }

    public DataState Reduce(DataState state, UserActions.Logout action)
    {
      state.Items = state.Items.Clear();
      return state;
    }
  }
}
