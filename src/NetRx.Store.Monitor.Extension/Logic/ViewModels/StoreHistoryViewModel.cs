using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NetRx.Store.Monitor.Extension.Logic.ViewModels
{
    public class StoreHistoryViewModel : BaseViewModel
    {
        private readonly ObservableCollection<HistoryRecordViewModel> _allActions = new ObservableCollection<HistoryRecordViewModel>();

        public StoreHistoryViewModel(string name, string initialActionName, string initialStateValueJson)
        {
            ApplyFilterCommand = new Command(ApplyFilter);

            Name = name;
            var record = new HistoryRecordViewModel(initialActionName, initialStateValueJson);
            _allActions.Add(record);
            FilteredActions.Add(record);
        }

        public ICommand ApplyFilterCommand { get; private set; }

        public string Name { get; private set; } = string.Empty;

        private string _actionFilter;
        public string ActionFilter
        {
            get => _actionFilter;
            set
            {
                _actionFilter = value;
                NotifyPropertyChanged(nameof(ActionFilter));
            }
        }

        public ObservableCollection<HistoryRecordViewModel> FilteredActions { get; private set; } = new ObservableCollection<HistoryRecordViewModel>();

        public ObservableCollection<StateNodeViewModel> State { get; private set; } = new ObservableCollection<StateNodeViewModel>();

        private HistoryRecordViewModel _selectedHistoryRecord;
        public HistoryRecordViewModel SelectedHistoryRecord
        {
            get => _selectedHistoryRecord;
            set
            {
                _selectedHistoryRecord = value;

                if (_selectedHistoryRecord != null)
                {
                    UpdateStateValues(_selectedHistoryRecord.State, State);
                }
                else
                {
                    State?.Clear();
                }

                NotifyPropertyChanged(nameof(SelectedHistoryRecord));
            }
        }

        public void AddRecord(string actionName, string stateValueJson)
        {
            var record = new HistoryRecordViewModel(actionName, stateValueJson);
            _allActions.Add(record);
            if (!string.IsNullOrWhiteSpace(ActionFilter) && record.ActionName.Contains(ActionFilter)
                || string.IsNullOrWhiteSpace(ActionFilter))
            {
                FilteredActions.Add(record);
            }
        }

        private void ApplyFilter()
        {
            var records = string.IsNullOrWhiteSpace(ActionFilter)
                ? _allActions
                : _allActions.Where(r => r.ActionName.Contains(ActionFilter));

            FilteredActions.Clear();
            foreach (var record in records)
                FilteredActions.Add(record);
        }

        private void UpdateStateValues(
            ObservableCollection<StateNodeViewModel> selectedState,
            ObservableCollection<StateNodeViewModel> currentState)
        {
            if (selectedState == null)
                return;

            if (selectedState.Count == 0)
            {
                currentState.Clear();
            }
            else
            {
                StateNodeViewModel selectedStateItem, currentStateItem;

                if (selectedState.Count < currentState.Count)
                {
                    int i = selectedState.Count;
                    while (i < currentState.Count)
                    {
                        currentState.RemoveAt(i);
                    }
                }

                int j = 0;
                while (j < currentState.Count)
                {
                    currentStateItem = currentState[j];
                    selectedStateItem = selectedState[j];

                    CopyValues(selectedStateItem, currentStateItem);

                    j++;
                }

                if (selectedState.Count > currentState.Count)
                {
                    int k = currentState.Count;
                    while (k < selectedState.Count)
                    {
                        selectedStateItem = selectedState[k++];
                        currentStateItem = new StateNodeViewModel();

                        CopyValues(selectedStateItem, currentStateItem);

                        currentState.Add(currentStateItem);
                    }
                }
            }
        }

        private void CopyValues(StateNodeViewModel source, StateNodeViewModel target)
        {
            target.Name = source.Name;
            target.Value = source.Value;

            if (source.Childs == null)
            {
                target.Childs?.Clear();
                target.Childs = null;
            }
            else
            {
                if (target.Childs == null)
                    target.Childs = new ObservableCollection<StateNodeViewModel>();

                UpdateStateValues(source.Childs, target.Childs);
            }
        }
    }
}
