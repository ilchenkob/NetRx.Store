using NetRx.Effects;
using SampleMVVM.Wpf.Models.Services;
using System.Threading.Tasks;

namespace SampleMVVM.Wpf.Models.State.Effects
{
  public class LoginEffect : Effect<UserActions.LoginStart, UserActions.LoginResult>
  {
    private readonly IAuthService _authService;

    public LoginEffect(IAuthService authService)
    {
      _authService = authService;
    }

    public override async Task<UserActions.LoginResult> Invoke(UserActions.LoginStart action)
    {
      try
      {
        var result = await _authService.Login(action.Payload);
        return new UserActions.LoginResult(result);
      }
      catch
      {
        return new UserActions.LoginResult(0);
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
