using System;
using System.Windows.Input;

namespace NetRx.Store.Monitor.Extension.Logic.ViewModels
{
    public class Command : ICommand
    {
        private Func<object, bool> _canExecute;
        private Action<object> _execute;

        public Command(Action execute, Func<bool> canExecute = null)
        {
            this._execute = (arg) => execute();

            if (canExecute != null)
                this._canExecute = (arg) => canExecute();
        }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
