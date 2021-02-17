using SearchIcd10.Utils;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SearchIcd10.ViewModels
{
    /// <summary>
    /// View Model Class for handling Output of the Search ICD Operation
    /// </summary>
    public class OutputVM : NotifyingModel
    {
        public static readonly PropertyChangedEventArgs TextChangedArgs = new PropertyChangedEventArgs("Text");

        private string m_Text;
        /// <summary>
        /// Gets or sets the Text for the output
        /// </summary>
        public string Text
        {
            get { return m_Text; }
            set { SetValue(ref m_Text, value, TextChangedArgs); }
        }

        /// <summary>
        /// Gets the command that is called to copy the output to the clipboard
        /// </summary>
        public ICommand CopyToClipboardCmnd { get; private set; }

        public ICommand AddAdditional { get; private set; }

        public OutputVM()
        {
            Text = String.Empty;
            CopyToClipboardCmnd = new ActionCommand(() => CopyToClipboard());
            AddAdditional = new ActionCommand(AddAdditionalCodes);
        }

        /// <summary>
        /// Copies the output to the clipboard in both plain-text and RTF
        /// </summary>
        public void CopyToClipboard()
        {
            Clipboard.Clear();
            Clipboard.SetText(Text, TextDataFormat.Text);
        }

        public void AddAdditionalCodes()
        {
            var newListsWin = new ListsWindow();
            newListsWin.Show();
        }
    }
}
