using System.Collections.Generic;

namespace VagabondLib.Files
{
    /// <summary>
    /// Exposes interface for open file dialog
    /// </summary>
    public interface IOpenFileDialog
    {
        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <returns>Filename string of selected file</returns>
        string ShowDialog();

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <param name="filters">Filters to apply for the view</param>
        /// <returns>Filename string of selected file</returns>
        string ShowDialog(IEnumerable<Filter> filters);
    }
}
