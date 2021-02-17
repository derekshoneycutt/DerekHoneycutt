using System.Windows;
using System.Windows.Controls;

namespace VagabondLib.Behaviors
{
    /// <summary>
    /// Behavior class to handle ScrollViewer extra functionality
    /// </summary>
    public class ScrollViewerBehavior
    {
        /// <summary>
        /// Property used to handle the VerticalOffset of a ScrollViewer's scrollbar
        /// </summary>
        public static DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset",
                                                typeof(double),
                                                typeof(ScrollViewerBehavior),
                                                new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));

        /// <summary>
        /// Sets the value for a VerticalOffset on a ScrollViewer
        /// </summary>
        /// <param name="target">The ScrollViewer to set the value for</param>
        /// <param name="value">The new value to set</param>
        public static void SetVerticalOffset(FrameworkElement target, double value)
        {
            target.SetValue(VerticalOffsetProperty, value);
        }

        /// <summary>
        /// Gets the value for a Vertical Offset on a ScrollViewer
        /// </summary>
        /// <param name="target">The ScrollViewer to get the value from</param>
        /// <returns>The current set VerticalOffset value for the ScrollViewer</returns>
        public static double GetVerticalOffset(FrameworkElement target)
        {
            return (double)target.GetValue(VerticalOffsetProperty);
        }

        //Called when the VerticalOffset is changed for a ScrollViewer -- scrolls the ScrollViewer to that newly set location
        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = target as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }
    }
}
