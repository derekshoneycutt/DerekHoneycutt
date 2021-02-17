using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcdDatabaseBuilder.CmsFiles.Models
{
    public class IcdCmTabCodes
    {
        public string SectionRange { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }

        public string ParentCode { get; set; }

        public IEnumerable<string> InclusionTerms { get; set; }

        public TabularAddCodes AddCodes { get; set; }
    }
}
