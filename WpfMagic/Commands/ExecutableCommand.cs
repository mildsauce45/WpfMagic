using System;
using System.Windows.Input;

namespace WpfMagic.Commands
{
    /// <summary>
    ///  Because we want to depend on no external libraries other than .NET, I've supplied a default implementation of the ICommand in much the same manner
    ///  as MVVM Light.
    /// </summary>
    public class ExecutableCommand : ICommand
    {
        private Action action;
        private Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public ExecutableCommand(Action action, Func<bool> canExecute = null)
        {
            this.action = action;
            this.canExecute = canExecute;
        }        

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                action();
        }
    }

    /// <summary>
    ///  Because we want to depend on no external libraries other than .NET, I've supplied a default implementation of the ICommand in much the same manner
    ///  as MVVM Light.
    /// </summary>
    public class ExecutableCommand<T> : ICommand
    {
        private Action<T> action;
        private Func<T, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public ExecutableCommand(Action<T> action, Func<T, bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }        
        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                action((T)parameter);
        }
    }
}
