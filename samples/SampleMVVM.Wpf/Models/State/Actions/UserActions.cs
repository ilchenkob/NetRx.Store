namespace SampleMVVM.Wpf.Models.State.UserActions
{
  public class LoginStart : NetRx.Store.Action<string>
  {
    public LoginStart(string payload) : base(payload)
    {
    }
  }

  public class LoginResult : NetRx.Store.Action<int>
  {
    public LoginResult(int payload) : base(payload)
    {
    }
  }

  public class Logout : NetRx.Store.Action
  {
  }
}
