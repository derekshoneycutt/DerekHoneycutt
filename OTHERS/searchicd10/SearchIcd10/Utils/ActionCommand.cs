using System;
using System.Windows.Input;

namespace SearchIcd10.Utils
{
    /// <summary>
    /// Defines a command interface for simple Action based commands
    /// </summary>
    public class ActionCommand : ICommand
    {
        private Action m_CommandDelegate;
        /// <summary>
        /// The command delegate that will be called when executed
        /// </summary>
        public Action CommandDelegate
        {
            get
            {
                return m_CommandDelegate;
            }
            set
            {
                if (!Object.Equals(m_CommandDelegate, value))
                {
                    m_CommandDelegate = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Construct a new ActionCommand with the given action
        /// </summary>
        /// <param name="inCommand">Action delegate to set to</param>
        public ActionCommand(Action inCommand)
        {
            CommandDelegate = inCommand;
        }

        /// <summary>
        /// Returns whether the object can be called
        /// </summary>
        /// <param name="parameter">Parameter expected to be sent to the delegate (ignored)</param>
        /// <returns>True if the delegate can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return (m_CommandDelegate != null);
        }

        /// <summary>
        /// Called when the state of CanExecute may return something different
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Execute the delegate
        /// </summary>
        /// <param name="parameter">Parameter expected to be sent to the delegate (ignored)</param>
        public void Execute(object parameter)
        {
            if (CommandDelegate != null)
            {
                CommandDelegate();
            }
        }
    }

    /// <summary>
    /// Defines a command interface for simple Action based commands with 1 parameter
    /// </summary>
    public class ActionCommand<T1> : ICommand
    {
        private Action<T1> m_CommandDelegate;
        /// <summary>
        /// The command delegate that will be called when executed
        /// </summary>
        public Action<T1> CommandDelegate
        {
            get
            {
                return m_CommandDelegate;
            }
            set
            {
                if (!Object.Equals(m_CommandDelegate, value))
                {
                    m_CommandDelegate = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Construct a new ActionCommand with the given action
        /// </summary>
        /// <param name="inCommand">Action delegate to set to</param>
        public ActionCommand(Action<T1> inCommand)
        {
            CommandDelegate = inCommand;
        }

        /// <summary>
        /// Returns whether the object can be called
        /// </summary>
        /// <param name="parameter">Parameter expected to be sent to the delegate (ignored here only)</param>
        /// <returns>True if the delegate can be executed</returns>
        public bool CanExecute(object parameter)
        {
            return (m_CommandDelegate != null);
        }

        /// <summary>
        /// Called when the state of CanExecute may return something different
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Execute the delegate
        /// </summary>
        /// <param name="parameter">Parameter expected to be sent to the delegate</param>
        public void Execute(object parameter)
        {
            if (CommandDelegate != null)
            {
                CommandDelegate((T1)parameter);
            }
        }
    }
}
