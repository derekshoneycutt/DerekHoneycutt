using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace VagabondLib.Collections
{
    public class RangedObservableCollection<T> : ObservableCollection<T>
    {
        public RangedObservableCollection() : base()
        {
        }

        public RangedObservableCollection(IEnumerable<T> items) : base(items)
        {
        }

        private void RaiseForChangedCollection(NotifyCollectionChangedAction notificationMode, IList changedItems)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            if (changedItems != null)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(notificationMode, changedItems));
            }
            else
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(notificationMode));
            }
        }

        private void RaiseForChangedCollection(NotifyCollectionChangedAction notificationMode)
        {
            RaiseForChangedCollection(notificationMode, null);
        }

        public void AddRange(IEnumerable<T> newItems, NotifyCollectionChangedAction notificationMode)
        {
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }
            if ((notificationMode != NotifyCollectionChangedAction.Add) && (notificationMode != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("Mode must be either Add or Reset for AddRange.", "notificationMode");
            }

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Add)
            {
                var newItemsList = newItems.ToList();
                foreach (var item in newItemsList)
                {
                    Items.Add(item);
                }
                RaiseForChangedCollection(NotifyCollectionChangedAction.Add, newItemsList);
            }
            else
            {
                foreach (var item in newItems)
                {
                    Items.Add(item);
                }
                RaiseForChangedCollection(NotifyCollectionChangedAction.Reset);
            }
        }

        public void AddRange(IEnumerable<T> newItems)
        {
            AddRange(newItems, NotifyCollectionChangedAction.Add);
        }

        public void RemoveRange(IEnumerable<T> oldItems, NotifyCollectionChangedAction notificationMode)
        {
            if (oldItems == null)
            {
                throw new ArgumentNullException("oldItems");
            }
            if ((notificationMode != NotifyCollectionChangedAction.Remove) && (notificationMode != NotifyCollectionChangedAction.Reset))
            {
                throw new ArgumentException("Mode must be either Remove or Reset for RemofeRange.", "notificationMode");
            }

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Remove)
            {
                var findItemsList = oldItems.ToList();
                var oldItemsList = new List<T>(findItemsList.Count);
                foreach (var item in findItemsList)
                {
                    if (Items.Remove(item))
                    {
                        oldItemsList.Add(item);
                    }
                }
                RaiseForChangedCollection(NotifyCollectionChangedAction.Remove, oldItemsList);
            }
            else
            {
                foreach (var item in oldItems)
                {
                    Items.Remove(item);
                }
                RaiseForChangedCollection(NotifyCollectionChangedAction.Reset);
            }
        }

        public void RemoveRange(IEnumerable<T> oldItems)
        {
            RemoveRange(oldItems, NotifyCollectionChangedAction.Remove);
        }

        public void Replace(IEnumerable<T> newItems)
        {
            if (newItems == null)
            {
                throw new ArgumentNullException("newItems");
            }

            Items.Clear();
            AddRange(newItems, NotifyCollectionChangedAction.Reset);
        }

        public void Replace(T newItem)
        {
            Replace(new[] { newItem });
        }
    }
}
