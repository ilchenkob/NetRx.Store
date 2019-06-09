using System.ComponentModel;

namespace NetRx.Store.Monitor.Extension.Logic.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
