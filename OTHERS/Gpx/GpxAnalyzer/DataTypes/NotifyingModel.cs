using System.Collections.Generic;
using System.ComponentModel;

namespace GpxAnalyzer.DataTypes
{
    /// <summary>
    /// Abstract class used to make easy use of the INotifyPropertyChanged interface
    /// </summary>
    public abstract class NotifyingModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event raised when object notifies of a change to one of its properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the PropertyChanged event once for each given PropertyChangedEventArgs object
        /// </summary>
        /// <param name="eventArgs">All Event Arguments to be passed to a raising of the event</param>
        protected virtual void RaisePropertyChanged(params PropertyChangedEventArgs[] eventArgs)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                foreach (var args in eventArgs)
                {
                    eventHandler(this, args);
                }
            }
        }

        /// <summary>
        /// Set a Value of a property, raising the appropriate events when necessary as well
        /// </summary>
        /// <typeparam name="TSetType">Type of the value being set</typeparam>
        /// <param name="obj">Member having the value being set</param>
        /// <param name="value">New value to set</param>
        /// <param name="e">Events to raise</param>
        /// <returns>True if new value has been set; false if values are already equal</returns>
        protected bool SetValue<TSetType>(ref TSetType obj, TSetType value, params PropertyChangedEventArgs[] e)
        {
            var ret = !EqualityComparer<TSetType>.Default.Equals(obj, value);
            if (ret)
            {
                obj = value;
                RaisePropertyChanged(e);
            }
            return ret;
        }
    }
}
