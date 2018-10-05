using SampleMVVM.Wpf.Models.State.Selectors;
using System;
using System.Windows.Input;
using UserActions = SampleMVVM.Wpf.Models.State.UserActions;

namespace SampleMVVM.Wpf.ViewModels
{
  public class UserViewModel : ViewModelBase
  {
    public UserViewModel()
    {
      SignInCommand = new RelayCommand(SignIn, CanSignIn);
      SignOutCommand = new RelayCommand(SignOut, CanSignOut);

      UserSelectors.Login.Subscribe(value => Login = value);
      UserSelectors.UserId.Subscribe(value => UserId = value);
    }

    public ICommand SignInCommand { get; private set; }

    public ICommand SignOutCommand { get; private set; }

    private string _login;
    public string Login
    {
      get
      {
        return _login;
      }
      set
      {
        _login = value;
        NotifyPropertyChanged();
      }
    }

    private int _userId;
    public int UserId
    {
      get
      {
        return _userId;
      }
      set
      {
        _userId = value;
        NotifyPropertyChanged();
        CommandManager.InvalidateRequerySuggested();
      }
    }

    private void SignIn()
    {
      App.Store.Dispatch(new UserActions.LoginStart(Login));
    }

    private bool CanSignIn() => !string.IsNullOrWhiteSpace(Login);

    private void SignOut()
    {
      App.Store.Dispatch(new UserActions.Logout());
    }

    private bool CanSignOut() => UserId > 0;
  }
}
