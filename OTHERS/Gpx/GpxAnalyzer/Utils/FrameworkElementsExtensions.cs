using GpxAnalyzer.DataTypes;
using System;
using System.Windows;

namespace GpxAnalyzer.Utils
{
    /// <summary>
    /// Extensions for FrameworkElement objects
    /// </summary>
    public static class FrameworkElementsExtensions
    {
        /// <summary>
        /// Setup for appropriate handling of IDisposable objects in WPF Windows
        /// <para>If used on a Window, will attach to Closed and Dispatcher.ShutdownStarted events to dispose of object</para>
        /// <para>If used on other UIElement, will search for parent Window in the Visual Tree and attach to that window's events</para>
        /// <para>If used on any other object, nothing will happen</para>
        /// </summary>
        /// <param name="fe">FrameworkElement object to attach to for Disposing of object</param>
        /// <param name="disposable">Disposable object to dispose of when appropriate</param>
        public static void HandleDisposable(this FrameworkElement fe, IDisposable disposable)
        {
            Action Dispose = () =>
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            };

            var win = fe as Window;
            if (win == null)
            {
                var uiEl = fe as UIElement;
                if (uiEl != null)
                {
                    win = UIElementHelper.FindParentOf<Window>(uiEl);
                }
            }

            if (win != null)
            {
                EventHandler WinClosed = null;
                EventHandler ShutdownStarted = null;

                WinClosed = (sender, e) =>
                {
                    Dispose();
                    win.Dispatcher.ShutdownStarted -= ShutdownStarted;
                    win.Closed -= WinClosed;
                };
                ShutdownStarted = (sender, e) =>
                {
                    Dispose();
                    win.Closed -= WinClosed;
                    win.Dispatcher.ShutdownStarted -= ShutdownStarted;
                };

                win.Closed += WinClosed;
                win.Dispatcher.ShutdownStarted += ShutdownStarted;
            }
        }

        /// <summary>
        /// Setup for appropriate handling of Disposable View Models in WPF
        /// <para>If used on a Window, will attach to Closed and Dispatcher.ShutdownStarted events to dispose of ViewModel</para>
        /// <para>If used on other UIElement, will search for parent Window in the Visual Tree and attach to that window's events</para>
        /// <para>If used on any other object, nothing will happen</para>
        /// </summary>
        /// <param name="fe">FrameworkElement object to attach to for Disposing of ViewModel (DataContext)</param>
        public static void HandleDisposableViewModel(this FrameworkElement fe)
        {
            var DataContext = fe.DataContext as IDisposable;
            if (DataContext != null)
            {
                HandleDisposable(fe, DataContext);
            }
        }

        /// <summary>
        /// Setup for appropriate handling of Disposable View Models in WPF
        /// <para>If used on a Window, will attach to Closed and Dispatcher.ShutdownStarted events to dispose of ViewModel</para>
        /// <para>If used on other UIElement, will search for parent Window in the Visual Tree and attach to that window's events</para>
        /// <para>If used on any other object, nothing will happen</para>
        /// <para>Also will dispose of one other IDisposable object</para>
        /// </summary>
        /// <param name="fe">FrameworkElement object to attach to for Disposing of ViewModel (DataContext) and object</param>
        /// <param name="other">Other element to dispose when appropriate</param>
        public static void HandleDisposableViewModel(this FrameworkElement fe, IDisposable other)
        {
            var composite = new CompositeDisposable();
            var DataContext = fe.DataContext as IDisposable;
            if (DataContext != null)
            {
                composite.Add(DataContext);
            }

            if (other != null)
            {
                composite.Add(other);
            }

            HandleDisposable(fe, composite);
        }
    }
}
