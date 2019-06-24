using System.Collections.ObjectModel;

namespace NetRx.Store.Monitor.Extension.Logic.ViewModels
{
    public class StateNodeViewModel : BaseViewModel
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private string _value = string.Empty;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyPropertyChanged(nameof(Value));
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = Childs != null ? value : false;
                NotifyPropertyChanged(nameof(IsExpanded));
            }
        }

        public ObservableCollection<StateNodeViewModel> Childs { get; set; } = new ObservableCollection<StateNodeViewModel>();
    }
}
