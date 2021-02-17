using System;
using System.Diagnostics;
using System.Windows.Input;

namespace VagabondLib.DataTypes
{
    public sealed class RelayCommand : ICommand
    {
        #region Fields 
        readonly Action _execute;
        readonly Func<bool> _canExecute;
        #endregion // Fields 
        #region Constructors 
        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute; _canExecute = canExecute;
        }
        #endregion // Constructors 
        #region ICommand Members 
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter) { _execute(); }
        #endregion // ICommand Members 
    }

    public sealed class RelayCommand<T> : ICommand
    {
        #region Fields 
        readonly Action<T> _execute;
        readonly Func<T, bool> _canExecute;
        #endregion // Fields 
        #region Constructors 
        public RelayCommand(Action<T> execute) : this(execute, null) { }
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute; _canExecute = canExecute;
        }
        #endregion // Constructors 
        #region ICommand Members 
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (parameter is T)
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }
            return false;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (parameter is T)
            {
                _execute((T)parameter);
            }
        }
        #endregion // ICommand Members 
    }
}
