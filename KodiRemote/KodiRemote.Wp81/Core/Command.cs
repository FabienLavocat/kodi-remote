using System;
using System.Windows.Input;

namespace KodiRemote.Wp81.Core
{
    public class Command : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _executeAction;
        private bool _canExecuteCache;

        private static readonly Func<object, bool> AlwaysExecute = o => true;

        public Command(Action<object> executeAction)
            : this(executeAction, AlwaysExecute)
        {
        }

        public Command(Action<object> executeAction, Func<object, bool> canExecute)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool temp = _canExecute(parameter);

            if (_canExecuteCache != temp)
            {
                _canExecuteCache = temp;
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }

            return _canExecuteCache;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}