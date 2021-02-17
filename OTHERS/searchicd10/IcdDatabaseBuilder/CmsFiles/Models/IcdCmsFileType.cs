using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcdDatabaseBuilder.CmsFiles.Models
{
    public enum IcdCmsFileType
    {
        None = 0,
        CmsTabularXml = 1,
        CmsIndexXml = 2,
        CmsAlternateTxt = 3,
        CmsGemsTxt = 4,
        CmsAlternateGemTxt = 5,
        PcsTabularXml = 6,
        PcsOrderTxt = 7,
        PcsIndexXml = 8,
        PcsAlternateTxt = 9,
        PcsGemsTxt = 10,
        PcsAlternateGemTxt = 11,
        XmlSpecialList = 12,
        SqliteFileSave = 13,
        //SqlServerSave = 14
        // Disabled SQL Server because relied on old things
    }
}
