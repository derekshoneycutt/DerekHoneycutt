using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SearchIcd10.Behaviors
{
    /// <summary>
    /// Class used to handle focus on a ScrollViewer to play nicely with Dragon Network Edition
    /// <para>Everything here is expected to be called from the main UI thread at all times; may not be safe on other threads</para>
    /// </summary>
    public class ScrollViewerFocusHandler
    {
        //Store mouse down events on UIElements
        private HashSet<UIElement> MouseDowns;

        /// <summary>
        /// Gets or Sets the ScrollView that should be in focus at all times
        /// </summary>
        public ScrollViewer ScrollView { get; set; }

        /// <summary>
        /// TextBox that provides exceptional behavior largely opposite that of other textboxes
        /// </summary>
        public TextBox ExceptedTextBox { get; set; }

        public ScrollViewerFocusHandler()
        {
            MouseDowns = new HashSet<UIElement>();

            ScrollView = null;
            ExceptedTextBox = null;
            //BlockingElement = null;
        }

        /// <summary>
        /// Handle when the PreviewMouseDown occurs on a UIElement--no action taken on any other sender objects
        /// </summary>
        /// <param name="sender">UIElement with mouse down</param>
        /// <param name="e">Mouse Button Event arguments</param>
        public void PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            var uiElement = sender as UIElement;
            if (uiElement != null)
            {
                if (!MouseDowns.Contains(uiElement))
                {
                    MouseDowns.Add(uiElement);
                    return;
                }
            }
        }

        /// <summary>
        /// Handle when the PreviewMouseUp occurs on a UIElement--no action taken on any other sender objects
        /// </summary>
        /// <param name="sender">UIElement with mouse up</param>
        /// <param name="e">Mouse Button Event arguments</param>
        public void PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
            var uiElement = sender as UIElement;
            if (uiElement != null)
            {
                if (MouseDowns.Contains(uiElement))
                {
                    MouseDowns.Remove(uiElement);
                    FocusScrollViewer();
                    return;
                }
            }
        }

        /// <summary>
        /// Event when a control has lost focus, meant to re-focus on ScrollView when appropriate
        /// </summary>
        /// <param name="sender">control that has lost focus</param>
        /// <param name="e">Event arguments passed</param>
        public void LostFocus(object sender, RoutedEventArgs e)
        {
            var focused = Keyboard.FocusedElement as UIElement;
            if (!object.ReferenceEquals(focused, ExceptedTextBox) && (ExceptedTextBox != null))
            {
                FocusScrollViewer();
            }
        }

        /// <summary>
        /// Event when a control's visibility is changed
        /// <para>This is most appropriate for the ScrollView item, as it should take focus when it is visible</para>
        /// </summary>
        /// <param name="sender">Control that visibility has changed on</param>
        /// <param name="e">Event arguments for the property changing</param>
        public void IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FocusScrollViewer();
        }

        /// <summary>
        /// Focus on the ScrollViewer when such is the appropriate action
        /// </summary>
        public void FocusScrollViewer()
        {
            if (ScrollView == null)
            {
                return;
            }

            if (!ScrollView.IsVisible)
            {
                return;
            }

            //Need to split off on a new thread to wait a tenth of a second; this is necessary in case strange things are happening
                //Returns to the main UI thread to actually focus, so no major interruptions should occur
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(100);
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var focused = Keyboard.FocusedElement as UIElement;
                        if (focused == null)
                        {
                            return;
                        }
                        if ((!(focused is TextBox) || object.ReferenceEquals(focused, ExceptedTextBox)) &&
                            !(((focused is CheckBox) || (focused is Button) || (focused is GridSplitter)) && (MouseDowns.Count > 0)))
                        {
                            ScrollView.Focus();
                        }
                    });
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    //We can ignore this here, because it just means that the window closed in the middle of scrolling... no worries!
                }
                catch (System.NullReferenceException)
                {
                    //There is the possibility of a null reference exception at this point... go ahead and ignore this, as the window is just closing
                }
            });
        }
    }
}
