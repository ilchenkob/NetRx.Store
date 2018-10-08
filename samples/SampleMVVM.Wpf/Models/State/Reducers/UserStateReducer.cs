using NetRx.Store;
using SampleMVVM.Wpf.Models.State.States;

namespace SampleMVVM.Wpf.Models.State.Reducers
{
  public class UserStateReducer : Reducer<UserState>
  {
    public UserState Reduce(UserState state, UserActions.LoginStart action)
    {
      state.IsLoading = true;
      return state;
    }

    public UserState Reduce(UserState state, UserActions.LoginResult action)
    {
      state.UserId = action.Payload;
      state.IsLoading = false;
      return state;
    }

    public UserState Reduce(UserState state, UserActions.Logout action)
    {
      state.UserId = 0;
      return state;
    }
  }
}
