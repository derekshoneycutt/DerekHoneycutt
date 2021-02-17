using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace VagabondLib.Behaviors
{
    /// <summary>
    /// Behavior class for buttons displaying a Drop down Menu
    /// <para>Will cause ContextMenu defined in button XAML to display when button is clicked</para>
    /// </summary>
    public class DropDownButton : Behavior<Button>
    {
        private bool IsContextMenuOpen;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AddHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click), true);
        }

        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var source = sender as Button;
            if (source != null && source.ContextMenu != null)
            {
                if (!IsContextMenuOpen)
                {
                    // Add handler to detect when the ContextMenu closes
                    source.ContextMenu.AddHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed), true);
                    // If there is a drop-down assigned to this button, then position and display it 
                    source.ContextMenu.PlacementTarget = source;
                    source.ContextMenu.Placement = PlacementMode.Bottom;
                    source.ContextMenu.IsOpen = true;
                    IsContextMenuOpen = true;
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RemoveHandler(Button.ClickEvent, new RoutedEventHandler(AssociatedObject_Click));
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            IsContextMenuOpen = false;
            var contextMenu = sender as ContextMenu;
            if (contextMenu != null)
            {
                contextMenu.RemoveHandler(ContextMenu.ClosedEvent, new RoutedEventHandler(ContextMenu_Closed));
            }
        }
    }
}
