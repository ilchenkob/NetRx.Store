using System;
using System.Threading.Tasks;

namespace SampleMVVM.Wpf.Models.Services
{
  public class AuthService : IAuthService
  {
    public async Task<int> Login(string login)
    {
      // simulate the waiting for the response
      await Task.Delay(TimeSpan.FromMilliseconds(700));

      return DateTime.Now.Second;
    }

    public Task Logout()
    {
      // simulate the waiting for the response
      return Task.Delay(TimeSpan.FromMilliseconds(600));
    }
  }
}