using System;
using System.Linq;

namespace GpxAnalyzer
{
    public sealed class FileSaveDialog : IIODialog
    {
        public string Show(IODialogInfo info)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.CheckPathExists = true;
            dlg.OverwritePrompt = true;

            if (info != null)
            {
                if (!String.IsNullOrWhiteSpace(info.Title))
                {
                    dlg.Title = info.Title;
                }

                if (info.Filters != null)
                {
                    var nonBlank = info.Filters.Where(f => !String.IsNullOrWhiteSpace(f)).ToList();
                    if (nonBlank.Count > 0)
                    {
                        dlg.Filter = String.Join("|", nonBlank);
                    }
                }
            }

            if (dlg.ShowDialog() == true)
            {
                return dlg.FileName;
            }

            return null;
        }
    }
}
