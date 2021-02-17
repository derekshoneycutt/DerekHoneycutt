using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GpxAnalyzer.Utils
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
        /// Find all children of a UIElement that are of a given type
        /// </summary>
        /// <typeparam name="T">Type of the child to find</typeparam>
        /// <param name="parent">The item to find the child of</param>
        /// <returns>The child elements searched for</returns>
        public static IEnumerable<T> GetChildren<T>(UIElement parent) where T : UIElement
        {
            UIElement child = null;
            for (int index = 0; index < VisualTreeHelper.GetChildrenCount(parent); ++index)
            {
                child = VisualTreeHelper.GetChild(parent, index) as UIElement;
                var useChild = child as T;
                if (useChild != null)
                {
                    yield return useChild;
                }
                else if (child != null)
                {
                    var retChildren = GetChildren<T>(child);
                    foreach (var retChild in retChildren)
                    {
                        yield return retChild;
                    }
                }
            }
        }

        /// <summary>
        /// Find a nearby item within a parent Grid
        /// <para>For example, a Button may have a Textbox nearby it in the same Grid, and this can find that</para>
        /// <para>Will always search for nearby within a Grid specifically</para>
        /// </summary>
        /// <typeparam name="T">Type of element to find nearby</typeparam>
        /// <typeparam name="TParentType">Type of the Parent element to search within</typeparam>
        /// <param name="fromEl">Element to find the other nearby element from</param>
        /// <returns>The nearby element, or null if none found</returns>
        public static T FindNear<T, TParentType>(UIElement fromEl)
            where T : UIElement
            where TParentType : UIElement
        {
            if (fromEl != null)
            {
                var grid = UIElementHelper.FindParentOf<TParentType>(fromEl);
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
        /// Find nearby items within a parent element
        /// <para>For example, a Button may have a Textbox nearby it in the same Grid, and this can find that</para>
        /// </summary>
        /// <typeparam name="T">Type of element to find nearby</typeparam>
        /// <typeparam name="TParentType">Type of the Parent element to search within</typeparam>
        /// <param name="fromEl">Element to find the other nearby element from</param>
        /// <returns>The nearby elements</returns>
        public static IEnumerable<T> FindAllNear<T, TParentType>(UIElement fromEl)
            where T : UIElement
            where TParentType : UIElement
        {
            if (fromEl != null)
            {
                var parent = UIElementHelper.FindParentOf<TParentType>(fromEl);
                if (parent != null)
                {
                    return UIElementHelper.GetChildren<T>(parent);
                }
            }
            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Attempt to focus on a nearby item of a given type
        /// </summary>
        /// <typeparam name="T">Type of element to focus on</typeparam>
        /// <typeparam name="TParentType">Type of the Parent element to search within</typeparam>
        /// <param name="fromEl">Element to find nearby element of</param>
        /// <returns>The nearby element that was focused, or null if not successful</returns>
        public static T FocusOnNear<T, TParentType>(UIElement fromEl)
            where T : UIElement
            where TParentType : UIElement
        {
            var near = FindNear<T, TParentType>(fromEl);
            if (near != null)
            {
                if (near.IsVisible)
                {
                    near.Focus();
                    return near;
                }
            }
            return null;
        }

        /// <summary>
        /// Try to find a given UIElement type from the specified point
        /// </summary>
        /// <typeparam name="T">UIElement type to find</typeparam>
        /// <param name="reference">Reference UIElement to find the point within</param>
        /// <param name="point">The point to find the UIElement at</param>
        /// <returns>The UIElement discovered, or null</returns>
        public static T FindFromPoint<T>(UIElement reference, System.Windows.Point point)
            where T : UIElement
        {
            var item = reference.InputHitTest(point);
            var el = item as UIElement;
            if (el == null)
            {
                return null;
            }

            var ret = el as T;
            if (ret != null)
            {
                return ret;
            }

            return FindParentOf<T>(el);
        }
    }
}
