using NetRx.Store.Monitor.Shared.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetRx.Store.Monitor.Extension.Logic.ViewModels
{
    public class MonitorToolViewModel : BaseViewModel
    {
        public ObservableCollection<StoreHistoryViewModel> StoreHistories { get; private set; } = new ObservableCollection<StoreHistoryViewModel>();

        private StoreHistoryViewModel _selectedStoreHistory;
        public StoreHistoryViewModel SelectedStoreHistory
        {
            get => _selectedStoreHistory;
            set
            {
                _selectedStoreHistory = value;
                NotifyPropertyChanged(nameof(SelectedStoreHistory));
            }
        }

        public void AddStateRecord(TraceMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"--- {message.ActionTypeName}");
            
            var stateHistory = StoreHistories.FirstOrDefault(r => r.Name == message.StoreTypeName);
            if (stateHistory == null)
            {
                var newStoreItem = new StoreHistoryViewModel(message.StoreTypeName, message.ActionTypeName, message.StateValueJson);
                StoreHistories.Add(newStoreItem);

                if (StoreHistories.Count == 1)
                    SelectedStoreHistory = newStoreItem;
            }
            else
            {
                stateHistory.AddRecord(message.ActionTypeName, message.StateValueJson);
            }
        }

        public void Clear()
        {
            StoreHistories.Clear();
        }
    }
}
