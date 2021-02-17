using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace VagabondLib.Behaviors
{
    /// <summary>
    /// Behavior class for special functions on HyperLink WPF Objects
    /// </summary>
    public static class HyperLinks
    {
        /// <summary>
        /// Property for the IsExternal behavior property
        /// </summary>
        public static readonly DependencyProperty IsExternalProperty =
            DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(HyperLinks), new UIPropertyMetadata(false, OnIsExternalChanged));

        /// <summary>
        /// Get the IsExternal property for an associated HyperLink
        /// </summary>
        /// <param name="obj">HyperLink object to get the property from</param>
        /// <returns>Current value of the IsExternal property</returns>
        public static bool GetIsExternal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExternalProperty);
        }

        /// <summary>
        /// Sets the IsExternal property for an associated HyperLink
        /// </summary>
        /// <param name="obj">HyperLink object to get the property from</param>
        /// <param name="value">New value of the IsExternal property</param>
        public static void SetIsExternal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExternalProperty, value);
        }

        private static void OnIsExternalChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            //Raised when the IsExternal value is changed on a HyperLink; (dis)connect the RequestNavigate event
            var hyperlink = sender as Hyperlink;

            if ((bool)args.NewValue)
            {
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
            }
            else
            {
                hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
            }
        }

        private static void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            //Raised when the hyperlink is "clicked" ; Navigate to the entered URI
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
