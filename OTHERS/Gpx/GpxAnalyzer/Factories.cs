using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalyzer
{
    public static class Factories
    {
        public static IIODialog GetOpenFileDialog()
        {
            return new FileOpenDialog();
        }

        public static IIODialog GetSaveFileDialog()
        {
            return new FileSaveDialog();
        }
    }
}
