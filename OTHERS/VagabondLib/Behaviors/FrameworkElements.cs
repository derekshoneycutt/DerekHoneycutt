using System.Windows;
using System.Windows.Input;

namespace VagabondLib.Behaviors
{
    /// <summary>
    /// Utility behavior class used to Handle special functions of a FrameworkElement object
    /// <para>Contains DoubleClickCommand and DoubleClickCommandParameter for handling double click to an ICommand</para>
    /// </summary>
    public class FrameworkElements : DependencyObject
    {
        /// <summary>
        /// Property to be set for the DoubleClickCommand property of a FrameworkElement
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(FrameworkElements),
                                                new PropertyMetadata(OnChangedDoubleClickCommand));

        /// <summary>
        /// Gets the ICommand set on the DoubleClickCommandProperty
        /// </summary>
        /// <param name="target">FrameworkElement that the property is set for</param>
        /// <returns>The ICommand set</returns>
        public static ICommand GetDoubleClickCommand(FrameworkElement target)
        {
            return (ICommand)target.GetValue(DoubleClickCommandProperty);
        }

        /// <summary>
        /// Sets the ICommand on the DoubleClickCommandProperty
        /// </summary>
        /// <param name="target">The FrameworkElement to set value for</param>
        /// <param name="value">The ICommand to set</param>
        public static void SetDoubleClickCommand(FrameworkElement target, ICommand value)
        {
            target.SetValue(DoubleClickCommandProperty, value);
        }

        //When the DoubleClickCommandProperty is changed, re-setup the MouseDown event that should be monitored to handle the clicks
        private static void OnChangedDoubleClickCommand(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FrameworkElement;
            if (control != null)
            {
                control.MouseDown -= control_MouseDown;
                if (e.NewValue != null)
                {
                    control.MouseDown += control_MouseDown;
                }
            }
        }

        private static void control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //When the user double-clicks on the item, the MouseDown event can be used to handle such an event.
            //When this occurs, we want to attempt to call the ICommand set, including sending any value set to the DoubleClickCommandParametersProperty below
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.ClickCount == 2)
                {
                    var control = sender as FrameworkElement;
                    if (control != null)
                    {
                        var command = GetDoubleClickCommand(control);

                        if (command != null)
                        {
                            if (command.CanExecute(null))
                            {
                                object param = GetDoubleClickCommandParameter(control);
                                command.Execute(param);
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Property used to handle parameters that should be sent to the DoubleClickCommandProperty above when it is called
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommandParameter", typeof(object), typeof(FrameworkElements),
                                                new PropertyMetadata(null));

        /// <summary>
        /// Gets the Command Parameters that have been set for the object
        /// </summary>
        /// <param name="target">FrameworkElement that this has been set for</param>
        /// <returns>The object previously set, or null</returns>
        public static object GetDoubleClickCommandParameter(FrameworkElement target)
        {
            return target.GetValue(DoubleClickCommandParameterProperty);
        }

        /// <summary>
        /// Sets the Command Parameters that have been set for the object
        /// </summary>
        /// <param name="target">FrameworkElement that this should set the property for</param>
        /// <param name="value">The new value to be passed to the DoubleClickCommandProperty ICommand when appropriate</param>
        public static void SetDoubleClickCommandParameter(FrameworkElement target, object value)
        {
            target.SetValue(DoubleClickCommandParameterProperty, value);
        }
    }
}
