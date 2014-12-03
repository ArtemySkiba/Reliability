using System;
using System.Windows.Input;

namespace reliability
{
    public class Command : ICommand
    {

        private Action action;

        public Command(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return action != null;
        }

        public void Execute(object parameter)
        {
            action();
        }

        public event EventHandler CanExecuteChanged;
    }
}
