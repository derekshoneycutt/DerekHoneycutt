using System.Collections.Generic;
using System.Windows;
using VagabondLib.Files;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Win32 Implementation of the Files.IOpenFileDialog for Files operations
    /// </summary>
    public class OpenFileDialog : IOpenFileDialog
    {
        /// <summary>
        /// Gets or Sets the window that will own the dialog, or null if none
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// Initialize the open file dialog window
        /// </summary>
        public OpenFileDialog()
        {
            Owner = null;
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <returns>Filename string of selected file</returns>
        public string ShowDialog()
        {
            return ShowDialog(new[] { Filter.AllFilesFilter });
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <param name="filters">Filters to apply for the view</param>
        /// <returns>Filename string of selected file</returns>
        public string ShowDialog(IEnumerable<Filter> filters)
        {
            bool? result = null;

            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = Filter.RangeToString(filters);

            if (Owner != null)
            {
                result = dlg.ShowDialog(Owner);
            }
            else
            {
                result = dlg.ShowDialog();
            }

            if (result == true)
            {
                return dlg.FileName;
            }
            return null;
        }
    }
}
