using System.Threading.Tasks;

namespace SampleMVVM.Wpf.Models.Services
{
  public interface IAuthService
  {
    Task<int> Login(string login);

    Task Logout();
  }
}