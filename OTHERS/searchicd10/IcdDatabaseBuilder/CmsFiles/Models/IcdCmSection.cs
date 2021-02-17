using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcdDatabaseBuilder.CmsFiles.Models
{
    public class IcdCmSection
    {
        public TabularAddCodes AddCodes { get; set; }

        public string CodeRange { get; set; }
        public string Title { get; set; }
    }
}
