using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalyzer
{
    public sealed class FileOpenDialog : IIODialog
    {
        public string Show(IODialogInfo info)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.CheckFileExists = true;
            
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
