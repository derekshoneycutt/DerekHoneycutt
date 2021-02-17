using System;
using System.Windows;

namespace VagabondLib.Utils
{
    /// <summary>
    /// Extensions for FrameworkElement objects
    /// </summary>
    public static class FrameworkElementsExtensions
    {
        /// <summary>
        /// Setup for appropriate handling of Disposable View Models in WPF
        /// <para>If used on a Window, will attach to Closed and Dispatcher.ShutdownStarted events to dispose of ViewModel</para>
        /// <para>If used on other UIElement, will search for parent Window in the Visual Tree and attach to that window's events</para>
        /// <para>If used on any other object, nothing will happen</para>
        /// </summary>
        /// <param name="fe">FrameworkElement object to attach to for Disposing of ViewModel (DataContext)</param>
        public static void HandleDisposableViewModel(this FrameworkElement fe)
        {
            Action Dispose = () =>
            {
                var DataContext = fe.DataContext as IDisposable;
                if (DataContext != null)
                {
                    DataContext.Dispose();
                }
            };

            var win = fe as Window;

            if (win == null)
            {
                var ctrl = fe as UIElement;
                if (ctrl != null)
                {
                    win = UIElementHelper.FindParentOf<Window>(ctrl);
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
    }
}
