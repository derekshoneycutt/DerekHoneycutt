using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// Class that stores a collection of ItemVM objects and notifies when the collection has changed. 
    /// Allows Blocking of the CollectionChanged event if appropriate
    /// </summary>
    public class NotifyingItemVMList : INotifyCollectionChanged, IEnumerable<ListItemVM>
    {
        /// <summary>
        /// Event raised when the collection has changed
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Gets or Sets whether to Raise the CollectionChanged event when appropriate
        /// </summary>
        private bool RaiseCollectionChanged { get; set; }

        /// <summary>
        /// Gets the underlying ObservableCollection.
        /// NOTE: If the CollectionChanged event on this property is caught, it will not be blocked by RaiseCollectionChanged being false
        /// </summary>
        public ObservableCollection<ListItemVM> Items { get; private set; }

        /// <summary>
        /// Initiate a new collection with no items
        /// </summary>
        public NotifyingItemVMList()
        {
            Items = new ObservableCollection<ListItemVM>();
            Items.CollectionChanged += Items_CollectionChanged;
        }

        /// <summary>
        /// Initiate a new collection with a given collection of existing items
        /// </summary>
        /// <param name="items">Collection of items to initiate with</param>
        public NotifyingItemVMList(IEnumerable<ListItemVM> items)
        {
            Items = new ObservableCollection<ListItemVM>(items);
            Items.CollectionChanged += Items_CollectionChanged;
        }

        /// <summary>
        /// Event raised when the Items property collection has changed. 
        /// Raises this class' CollectionChanged event if available and RaiseCollectionChanged is true
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event Arguments associated to the event</param>
        void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((CollectionChanged != null) && RaiseCollectionChanged)
            {
                CollectionChanged(this, e);
            }
        }

        /// <summary>
        /// Stop the CollectionChanged event from being raised in the future (does not stop from being raised by Items field)
        /// </summary>
        public void InterruptCollectionChanged()
        {
            RaiseCollectionChanged = false;
        }

        /// <summary>
        /// Allow the CollectionChanged event to be raised in the future
        /// </summary>
        public void AllowCollectionChanged()
        {
            RaiseCollectionChanged = true;
        }

        /// <summary>
        /// Force the CollectionChanged event to be raised as a Reset event
        /// </summary>
        public void ForceCollectionChanged()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (CollectionChanged != null)
                {
                    var newEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                    CollectionChanged(this, newEventArgs);
                }
            }));
        }

        /// <summary>
        /// Get the enumerator of the collection
        /// </summary>
        /// <returns>Enumerator for the collection</returns>
        public IEnumerator<ListItemVM> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Get the enumerator of the collection
        /// </summary>
        /// <returns>Enumerator for the collection</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
