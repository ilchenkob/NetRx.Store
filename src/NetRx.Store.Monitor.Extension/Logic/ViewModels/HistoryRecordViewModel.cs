using System;
using System.Collections.ObjectModel;

namespace NetRx.Store.Monitor.Extension.Logic.ViewModels
{
    public class HistoryRecordViewModel : BaseViewModel
    {
        public HistoryRecordViewModel(string actionName, string stateValueJson)
        {
            var actionNameParts = actionName.Split('.');
            ActionName = actionNameParts[actionNameParts.Length - 1];
            ActionFullName = actionName;

            State = StateValueParser.ParseJsonStateValue(stateValueJson);
        }

        public string ActionName { get; private set; } = string.Empty;

        public string ActionFullName { get; private set; } = string.Empty;

        public string ReceivedAt { get; private set; } = DateTime.Now.ToString("HH:mm:ss.fff");

        public ObservableCollection<StateNodeViewModel> State { get; private set; } = new ObservableCollection<StateNodeViewModel>();
    }
}
