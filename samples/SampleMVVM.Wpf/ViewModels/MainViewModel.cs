using System;
using SampleMVVM.Wpf.Models.State.Selectors;

namespace SampleMVVM.Wpf.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    public MainViewModel()
    {
      DataVM = new DataViewModel();
      UserVM = new UserViewModel();

      RootSelectors.IsLoading.Subscribe(value => IsLoading = value);
    }

    public DataViewModel DataVM { get; private set; }

    public UserViewModel UserVM { get; private set; }

    private bool _loading;
    public bool IsLoading
    {
      get
      {
        return _loading;
      }
      private set
      {
        _loading = value;
        NotifyPropertyChanged();
      }
    }
  }
}
