using System;
using System.Threading;

namespace GpxAnalyzer.DataTypes
{
    /// <summary>
    /// Class used to run an Action at the time of Disposal
    /// </summary>
    public sealed class AnonymousDisposable : IDisposable
    {
        private readonly Action m_Action;
        private int m_IsDisposed;

        /// <summary>
        /// Initiate a new AnonymousDisposable object with a given action
        /// </summary>
        /// <param name="act">Action to run at disposal</param>
        public AnonymousDisposable(Action act)
        {
            m_Action = act;
            m_IsDisposed = 0;
        }

        /// <summary>
        /// Dispose the object; Runs the action passed at construction
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref m_IsDisposed, 1) == 0)
            {
                if (m_Action != null)
                {
                    m_Action();
                }
            }
        }
    }
}
