using NetRx.Store;
using SampleMVVM.Wpf.Models.State.States;
using System.Collections.Immutable;
using System.Linq;

namespace SampleMVVM.Wpf.Models.State.Reducers
{
  public class DataStateReducer
  {
    public static DataState Function(DataState state, Action action)
    {
      if (action is DataActions.LoadDataStart)
      {
        state.IsLoading = true;
        return state;
      }
      if (action is DataActions.LoadDataResult loadResult)
      {
        state.Items = loadResult.Payload.ToImmutableList();
        state.IsLoading = false;
        return state;
      }
      if (action is DataActions.SendItemStart)
      {
        state.IsLoading = true;
        return state;
      }
      if (action is DataActions.SendItemResult sendResult)
      {
        if (sendResult.Payload.Id > 0)
          state.Items = state.Items.Remove(state.Items.First(d => d.Id == sendResult.Payload.Id));

        state.IsLoading = false;
        return state;
      }
      if (action is UserActions.Logout)
      {
        state.Items = state.Items.Clear();
        return state;
      }
      return state;
    }
  }
}
