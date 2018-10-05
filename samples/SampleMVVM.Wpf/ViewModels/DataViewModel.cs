using SampleMVVM.Wpf.Models.Entities;
using SampleMVVM.Wpf.Models.State.Selectors;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DataActions = SampleMVVM.Wpf.Models.State.DataActions;

namespace SampleMVVM.Wpf.ViewModels
{
  public class DataViewModel : ViewModelBase
  {
    private int _userId;
    private bool _isLoggedIn;

    public DataViewModel()
    {
      LoadCommand = new RelayCommand(Load, CanLoadAndSend);
      SendCommand = new RelayCommand(Send, CanLoadAndSend);

      Items = new ObservableCollection<DataItem>();

      UserSelectors.IsLoggedIn.Subscribe(value => _isLoggedIn = value);
      UserSelectors.UserId.Subscribe(value => _userId = value);
      DataSelectors.Items.Subscribe(items => Items = new ObservableCollection<DataItem>(items));
    }

    public ICommand LoadCommand { get; private set; }

    public ICommand SendCommand { get; private set; }

    private ObservableCollection<DataItem> _items;
    public ObservableCollection<DataItem> Items
    {
      get
      {
        return _items;
      }
      private set
      {
        _items = value;
        NotifyPropertyChanged();
      }
    }

    public DataItem SelectedItem { get; set; }

    public void Send()
    {
      App.Store.Dispatch(new DataActions.SendItemStart(SelectedItem));
    }

    public void Load()
    {
      App.Store.Dispatch(new DataActions.LoadDataStart(_userId));
    }

    public bool CanLoadAndSend() => _isLoggedIn;
  }
}
