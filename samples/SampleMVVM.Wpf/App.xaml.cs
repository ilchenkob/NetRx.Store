using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NetRx.Store;
using SampleMVVM.Wpf.Models.Services;
using SampleMVVM.Wpf.Models.State.Effects;
using SampleMVVM.Wpf.Models.State.Reducers;
using SampleMVVM.Wpf.Models.State.States;

namespace SampleMVVM.Wpf
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static Store Store { get; private set; }

    public App()
    {
      var authService = new AuthService();
      var dataService = new DataService();

      var effects = UserEffects.GetAll(authService)
                              .Concat(DataEffects.GetAll(dataService))
                              .Concat(MessageEffects.GetAll());

      Store = Store.Create()
                   .WithState(DataState.Initial(), new DataStateReducer())
                   .WithState(UserState.Initial(), new UserStateReducer())
                   .WithEffects(effects);
    }

    public static Task ShowMessge(string message, bool isError = false)
    {
      MessageBox.Show(
        message,
        "NetRx.Store sample",
        MessageBoxButton.OK,
        isError ? MessageBoxImage.Error : MessageBoxImage.Information);
      return Task.CompletedTask;
    }
  }
}
