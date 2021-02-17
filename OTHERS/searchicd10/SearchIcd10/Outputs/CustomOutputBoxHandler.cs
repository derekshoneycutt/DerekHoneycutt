using SearchIcd10.ViewModels;
using System;
using System.Windows;

namespace SearchIcd10.Outputs
{
    /// <summary>
    /// Displays the output in the custom output box
    /// </summary>
    public class CustomOutputBoxHandler : IOutputHandler
    {
        private static OutputWindow useOutputWindow = null;

        /// <summary>
        /// Puts the output desired
        /// </summary>
        /// <param name="outputText">Text to output</param>
        /// <returns>True if operation was successful</returns>
        public bool PutOutputText(string outputText)
        {
            if (useOutputWindow == null)
            {
                useOutputWindow = new OutputWindow(outputText);
            }
            else
            {
                var outputVM = (OutputVM)useOutputWindow.DataContext;
                outputVM.Text += Environment.NewLine + outputText;
            }
            useOutputWindow.Show();
            Application.Current.MainWindow = useOutputWindow;
            return true;
        }


        /// <summary>
        /// Switch to the existing window, if applicable to the output
        /// </summary>
        public void SwitchToExisting()
        {
            if (useOutputWindow != null)
            {
                useOutputWindow.Show();
                Application.Current.MainWindow = useOutputWindow;
            }
        }
    }
}
