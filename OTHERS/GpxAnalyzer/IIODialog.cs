using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalyzer
{
    public sealed class IODialogInfo
    {
        public string Title { get; set; }
        public string[] Filters { get; set; }
    }

    public interface IIODialog
    {
        string Show(IODialogInfo info);
    }
}
