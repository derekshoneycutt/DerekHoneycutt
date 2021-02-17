using System;
using System.Collections.Generic;
using System.Linq;

namespace GpxAnalyzer.DataTypes
{
    /// <summary>
    /// Class used to manage multiple disposable objects at once
    /// </summary>
    public sealed class CompositeDisposable : IEnumerable<IDisposable>, IDisposable
    {
        private List<IDisposable> Disposables;
        private object disposablesLock;

        /// <summary>
        /// Gets whether the object has been disposed already
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Initiate the object, empty
        /// </summary>
        public CompositeDisposable()
        {
            IsDisposed = false;
            Disposables = new List<IDisposable>();
            disposablesLock = new object();
        }

        /// <summary>
        /// Initiate the object with a set of existing disposable objects
        /// </summary>
        /// <param name="disposables">Disposable objects to initiate with</param>
        public CompositeDisposable(IEnumerable<IDisposable> disposables)
        {
            IsDisposed = false;
            Disposables = new List<IDisposable>(disposables);
            disposablesLock = new object();
        }

        /// <summary>
        /// Initiate the object with a set of existing disposable objects
        /// </summary>
        /// <param name="disposables">Disposable objects to initiate with</param>
        public CompositeDisposable(params IDisposable[] disposables)
        {
            IsDisposed = false;
            Disposables = new List<IDisposable>(disposables);
            disposablesLock = new object();
        }

        /// <summary>
        /// Add a new disposable object
        /// </summary>
        /// <param name="disposable">New disposable object to add and manage</param>
        public void Add(IDisposable disposable)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            lock (disposablesLock)
            {
                Disposables.Add(disposable);
            }
        }

        /// <summary>
        /// Add a new collection of disposable objects
        /// </summary>
        /// <param name="disposables">Collection of new disposable objects to add and manage</param>
        public void AddRange(IEnumerable<IDisposable> disposables)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            lock (disposablesLock)
            {
                Disposables.AddRange(disposables);
            }
        }

        /// <summary>
        /// Add a new collection of disposable objects
        /// </summary>
        /// <param name="disposables">Collection of new disposable objects to add and manage</param>
        public void AddRange(params IDisposable[] disposables)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            lock (disposablesLock)
            {
                Disposables.AddRange(disposables);
            }
        }

        /// <summary>
        /// Add a new Action to be run at time of disposal
        /// </summary>
        /// <param name="dispose">Action to run at disposal</param>
        /// <returns>A disposable object that, when disposed, will run the given action</returns>
        public IDisposable Add(Action dispose)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            IDisposable ret = null;
            lock (disposablesLock)
            {
                ret = new AnonymousDisposable(dispose);
                Disposables.Add(ret);
            }
            return ret;
        }

        /// <summary>
        /// Add a new collection of Actions to be run at time of disposal
        /// </summary>
        /// <param name="disposes">Collection of Actions to run at disposal</param>
        /// <returns>A Collaction of Disposable objects that, when disposed, will run the given actions</returns>
        public IEnumerable<IDisposable> AddRange(IEnumerable<Action> disposes)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            var ret = disposes.Select(d => new AnonymousDisposable(d)).ToList();
            lock (disposablesLock)
            {
                Disposables.AddRange(ret);
            }
            return ret.AsReadOnly();
        }

        /// <summary>
        /// Add a new collection of Actions to be run at time of disposal
        /// </summary>
        /// <param name="disposes">Collection of Actions to run at disposal</param>
        /// <returns>A Collaction of Disposable objects that, when disposed, will run the given actions</returns>
        public IEnumerable<IDisposable> AddRange(params Action[] disposes)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            var ret = disposes.Select(d => new AnonymousDisposable(d)).ToList();
            lock (disposablesLock)
            {
                Disposables.AddRange(ret);
            }
            return ret.AsReadOnly();
        }

        /// <summary>
        /// Add a special Action set, including action to run at addition and disposal, with a given handler object
        /// </summary>
        /// <typeparam name="TDelegate">Type of handler object to pass to the add and remove actions</typeparam>
        /// <param name="add">Action to be run on addition of the new disposable</param>
        /// <param name="remove">Action to be run on disposal of the new disposable</param>
        /// <param name="handler">Object to be passed to the add and remove actions</param>
        /// <returns>New Disposable object that will run the remove Action on disposal</returns>
        public IDisposable Add<TDelegate>(Action<TDelegate> add, Action<TDelegate> remove, TDelegate handler)
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            IDisposable ret = null;
            lock (disposablesLock)
            {
                if (add != null)
                {
                    add(handler);
                }
                ret = new AnonymousDisposable(() => remove(handler));
                Disposables.Add(ret);
            }
            return ret;
        }

        /// <summary>
        /// Clear out the disposals, in an unprotected context
        /// </summary>
        private void UClear()
        {
            var disposables = Disposables.ToArray();
            Disposables.Clear();
            foreach (var disposable in disposables)
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Clear and Dispose all object currently being managed
        /// </summary>
        public void Clear()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            lock (disposablesLock)
            {
                UClear();
            }
        }

        /// <summary>
        /// Get enumerator for all objects currently contained. Not thread safe
        /// </summary>
        /// <returns>Enumerator for the disposables</returns>
        public IEnumerator<IDisposable> GetEnumerator()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CompositeDisposable");
            }

            return Disposables.GetEnumerator();
        }

        /// <summary>
        /// Get enumerator for all objects currently contained. Not thread safe
        /// </summary>
        /// <returns>Enumerator for the disposables</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Dispose all objects in finality ; No further operations supported beyond this call
        /// </summary>
        public void Dispose()
        {
            lock (disposablesLock)
            {
                if (!IsDisposed)
                {
                    UClear();
                    IsDisposed = true;
                }
            }
        }
    }
}
