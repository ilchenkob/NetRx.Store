namespace SampleMVVM.Wpf.Models.State.UserActions
{
  public class LoginStart : NetRx.Store.Action<string>
  {
    public LoginStart(string payload) : base(payload)
    {
    }
  }

  public class LoginSuccess : NetRx.Store.Action<int>
  {
    public LoginSuccess(int payload) : base(payload)
    {
    }
  }

  public class LoginFailed : NetRx.Store.Action
  {
  }

  public class Logout : NetRx.Store.Action
  {
  }
}
