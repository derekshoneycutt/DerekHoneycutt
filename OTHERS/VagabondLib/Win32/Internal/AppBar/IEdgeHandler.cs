
namespace VagabondLib.Win32.Internal.AppBar
{
    /// <summary>
    /// Internal interface for handling specific functions on AppBar according to a specific edge of the screen
    /// </summary>
    internal interface IEdgeHandler
    {
        /// <summary>
        /// The edge that corresponds to this Edge handler
        /// </summary>
        ScreenEdge ThisEdge { get; }

        /// <summary>
        /// Get a new Rectangle to fill the AppBar according to a screen rectangle
        /// </summary>
        /// <param name="fromRect">Screen rectangle to get new AppBar area from</param>
        /// <param name="dimension">Dimension to use for the AppBar</param>
        /// <returns>New Rectangle representing the area of the new AppBar</returns>
        System.Drawing.Rectangle GetAppBarRectFromScreenRect(System.Drawing.Rectangle fromRect, int dimension);

        /// <summary>
        /// Get the current variable dimension from an AppBar's rectangle
        /// </summary>
        /// <param name="fromRect">Rectangle to get the variable dimension from</param>
        /// <returns>Integer value of the current variable dimension of the AppBar</returns>
        int GetCurrentDimension(System.Drawing.Rectangle fromRect);

        /// <summary>
        /// Fix a form's rectangle for resizing on an AppBar's window
        /// </summary>
        /// <param name="origBounds">Original Rectangle Bounds of the window prior to resizing</param>
        /// <param name="newSize">Projected new size of the window after resizing</param>
        /// <returns>New rectangle representing fixed resizing of the window</returns>
        RECT FixResizing(System.Drawing.Rectangle origBounds, RECT newSize);

        /// <summary>
        /// Fix an AppBar's rectangle when finished resizing the form
        /// </summary>
        /// <param name="fixRect">AppBar Rectangle to fix according to the resize data</param>
        /// <param name="dimension">New variable dimension value for the AppBar post-resizing</param>
        /// <returns>New rectangle with fixed dimensions post-resizing</returns>
        System.Drawing.Rectangle FixResizeEnd(System.Drawing.Rectangle fixRect, int dimension);

        /// <summary>
        /// When another AppBar causes an AppBar's location or size to change, the AppBar's rectangle needs to be adjusted
        /// to the appropriate new values. Discovers and returns these new values
        /// </summary>
        /// <param name="changedRect">Original AppBar Rectangle that needs to be adjusted to new values</param>
        /// <param name="dimension">Variable dimension value of the AppBar</param>
        /// <param name="appbar">Win32Api AppBar structure used to handle the lower-level AppBar processes</param>
        /// <returns>New Rectangle with the new, fixed values for the AppBar</returns>
        System.Drawing.Rectangle FixChangedRect(System.Drawing.Rectangle changedRect, int dimension, AppBar appbar);
    }
}
