using NetRx.Effects;
using SampleMVVM.Wpf.Models.Services;
using System.Threading.Tasks;

namespace SampleMVVM.Wpf.Models.State.Effects
{
  public class LoginEffect : Effect<UserActions.LoginStart, NetRx.Store.Action>
  {
    private readonly IAuthService _authService;

    public LoginEffect(IAuthService authService)
    {
      _authService = authService;
    }

    public override async Task<NetRx.Store.Action> Invoke(UserActions.LoginStart action)
    {
      try
      {
        var result = await _authService.Login(action.Payload);
        return new UserActions.LoginSuccess(result);
      }
      catch
      {
        return new UserActions.LoginFailed();
      }
    }
  }

  public class LogoutEffect : Effect<UserActions.Logout>
  {
    private readonly IAuthService _authService;

    public LogoutEffect(IAuthService authService)
    {
      _authService = authService;
    }

    public override Task Invoke(UserActions.Logout action)
    {
      return _authService.Logout();
    }
  }

  public static class UserEffects
  {
    public static Effect[] GetAll(IAuthService authService)
    {
      return new Effect[]
      {
        new LogoutEffect(authService),
        new LoginEffect(authService),
      };
    }
  }
}
