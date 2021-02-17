using System.Windows.Forms;

namespace VagabondLib.Win32.Internal.AppBar
{
    /// <summary>
    /// Handles edge-specific methods for AppBars docked to the left side of the screen
    /// </summary>
    internal sealed class LeftEdgeHandler : IEdgeHandler
    {
        /// <summary>
        /// The edge that corresponds to this Edge handler
        /// </summary>
        public ScreenEdge ThisEdge { get { return ScreenEdge.Left; } }

        /// <summary>
        /// Get a new Rectangle to fill the AppBar according to a screen rectangle
        /// </summary>
        /// <param name="fromRect">Screen rectangle to get new AppBar area from</param>
        /// <param name="dimension">Dimension to use for the AppBar</param>
        /// <returns>New Rectangle representing the area of the new AppBar</returns>
        public System.Drawing.Rectangle GetAppBarRectFromScreenRect(System.Drawing.Rectangle fromRect, int dimension)
        {
            var retRect = new System.Drawing.Rectangle();
            retRect.X = fromRect.X;
            retRect.Width = dimension;
            retRect.Y = fromRect.Y;
            retRect.Height = fromRect.Height;

            var getScreen = Screen.FromRectangle(retRect);
            retRect.Y = getScreen.WorkingArea.Y;
            retRect.Height = getScreen.WorkingArea.Height;
            retRect.X = getScreen.WorkingArea.X;
            retRect.Width = dimension;

            return retRect;
        }

        /// <summary>
        /// Get the current variable dimension from an AppBar's rectangle
        /// </summary>
        /// <param name="fromRect">Rectangle to get the variable dimension from</param>
        /// <returns>Integer value of the current variable dimension of the AppBar</returns>
        public int GetCurrentDimension(System.Drawing.Rectangle fromRect)
        {
            return fromRect.Width;
        }

        /// <summary>
        /// Fix a form's rectangle for resizing on an AppBar's window
        /// </summary>
        /// <param name="origBounds">Original Rectangle Bounds of the window prior to resizing</param>
        /// <param name="newSize">Projected new size of the window after resizing</param>
        /// <returns>New rectangle representing fixed resizing of the window</returns>
        public RECT FixResizing(System.Drawing.Rectangle origBounds, RECT newSize)
        {
            var ret = new RECT();

            ret.Top = origBounds.Top;
            ret.Bottom = origBounds.Bottom;
            if (newSize.Left != origBounds.Left)
            {
                ret.Right = origBounds.Right;
            }
            else
            {
                ret.Right = newSize.Right;
            }
            ret.Left = origBounds.Left;

            return ret;
        }

        /// <summary>
        /// Fix an AppBar's rectangle when finished resizing the form
        /// </summary>
        /// <param name="fixRect">AppBar Rectangle to fix according to the resize data</param>
        /// <param name="dimension">New variable dimension value for the AppBar post-resizing</param>
        /// <returns>New rectangle with fixed dimensions post-resizing</returns>
        public System.Drawing.Rectangle FixResizeEnd(System.Drawing.Rectangle fixRect, int dimension)
        {
            var retRect = new System.Drawing.Rectangle(fixRect.X, fixRect.Y, fixRect.Width, fixRect.Height);

            retRect.Width = dimension;

            return retRect;
        }

        /// <summary>
        /// When another AppBar causes an AppBar's location or size to change, the AppBar's rectangle needs to be adjusted
        /// to the appropriate new values. Discovers and returns these new values
        /// </summary>
        /// <param name="changedRect">Original AppBar Rectangle that needs to be adjusted to new values</param>
        /// <param name="dimension">Variable dimension value of the AppBar</param>
        /// <param name="appbar">Win32Api AppBar structure used to handle the lower-level AppBar processes</param>
        /// <returns>New Rectangle with the new, fixed values for the AppBar</returns>
        public System.Drawing.Rectangle FixChangedRect(System.Drawing.Rectangle changedRect, int dimension, AppBar appbar)
        {
            var retRect = new System.Drawing.Rectangle(changedRect.X, changedRect.Y, changedRect.Width, changedRect.Height);

            var getScreen = Screen.FromRectangle(retRect);

            if (getScreen.WorkingArea.Height != retRect.Height)
            {
                retRect.Y = getScreen.WorkingArea.Y;
                retRect.Height = getScreen.WorkingArea.Height;
            }

            if (retRect.Left != getScreen.Bounds.Left)
            {
                var newRect = appbar.QueryPos(ScreenEdge.Left, retRect);
                if (newRect.Left == retRect.Left)
                {
                    newRect.X = getScreen.Bounds.Left;
                    newRect.Width = dimension;
                    newRect = appbar.QueryPos(ScreenEdge.Left, newRect);
                }

                if (newRect.Left != retRect.Left)
                {
                    retRect.X = newRect.X;
                    retRect.Width = dimension;
                }
            }

            return retRect;
        }
    }
}
