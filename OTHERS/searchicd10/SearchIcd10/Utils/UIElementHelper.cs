using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SearchIcd10.Utils
{
    /// <summary>
    /// Utility Class for handling special UIElement requests
    /// </summary>
    internal static class UIElementHelper
    {
        /// <summary>
        /// Find the first parent of a UIElement that is of a given type
        /// </summary>
        /// <typeparam name="T">Type of the parent to find</typeparam>
        /// <param name="child">The item to find the parent of</param>
        /// <returns>The parent element searched for, or null if not found</returns>
        public static T FindParentOf<T>(UIElement child) where T : UIElement
        {
            if (child == null)
            {
                return null;
            }

            DependencyObject tempObj = child;
            T ret = null;
            while (ret == null)
            {
                tempObj = VisualTreeHelper.GetParent(tempObj);
                if (tempObj == null)
                {
                    return null;
                }
                ret = tempObj as T;
            }
            return ret;
        }

        /// <summary>
        /// Determine if one element is somehow a parent of another in the Visual Tree
        /// </summary>
        /// <param name="child">Anticipated Child element</param>
        /// <param name="parent">Anticipated Parent element</param>
        /// <returns>True if parent is higher in the Visual Tree of child; False otherwise</returns>
        public static bool IsParent(UIElement child, UIElement parent)
        {
            DependencyObject tempObj = child;
            while (tempObj != null)
            {
                tempObj = VisualTreeHelper.GetParent(tempObj);
                if (Object.ReferenceEquals(tempObj as UIElement, parent))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Find the first child of a UIElement that is of a given type
        /// </summary>
        /// <typeparam name="T">Type of the child to find</typeparam>
        /// <param name="parent">The item to find the child of</param>
        /// <returns>The child element searched for, or null if not found</returns>
        public static T GetChild<T>(UIElement parent) where T : UIElement
        {
            UIElement child = null;
            for (int index = 0; index < VisualTreeHelper.GetChildrenCount(parent); ++index)
            {
                child = VisualTreeHelper.GetChild(parent, index) as UIElement;
                var useChild = child as T;
                if (useChild != null)
                {
                    return useChild;
                }
                else if (child != null)
                {
                    useChild = GetChild<T>(child);
                    if (useChild != null)
                    {
                        return useChild;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Find a nearby item within a parent Grid
        /// <para>For example, a Button may have a Textbox nearby it in the same Grid, and this can find that</para>
        /// <para>Will always search for nearby within a Grid specifically</para>
        /// </summary>
        /// <typeparam name="T">Type of element to find nearby</typeparam>
        /// <param name="fromEl">Element to find the other nearby element from</param>
        /// <returns>The nearby element, or null if none found</returns>
        public static T FindNear<T>(UIElement fromEl) where T : UIElement
        {
            if (fromEl != null)
            {
                var grid = UIElementHelper.FindParentOf<Grid>(fromEl);
                if (grid != null)
                {
                    var near = UIElementHelper.GetChild<T>(grid);
                    if (near != null)
                    {
                        return near;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Attempt to focus on a nearby item of a given type
        /// </summary>
        /// <typeparam name="T">Type of element to focus on</typeparam>
        /// <param name="fromEl">Element to find nearby element of</param>
        /// <returns>The nearby element that was focused, or null if not successful</returns>
        public static T FocusOnNear<T>(UIElement fromEl) where T : UIElement
        {
            var near = FindNear<T>(fromEl);
            if (near != null)
            {
                near.Focus();
                return near;
            }
            return null;
        }
    }
}
