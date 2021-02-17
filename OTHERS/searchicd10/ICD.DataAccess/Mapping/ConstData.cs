using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess.Mapping
{
    /// <summary>
    /// Internal class used to map ObjectModels to the appropriate Tables/Columns 
    /// Each table is organized in this class as Table_x Where x is the Name of the Object minus the leading 'Icd' 
    /// Each Column is organized in this class as Table_x_y Where x is the Table Name and y is the Property Name
    /// </summary>
    public static class ConstData
    {
        public static readonly string Table_Terms = "SearchIcd_Terms";

        public static readonly string Table_Terms_Id = "Id";
        public static readonly string Table_Terms_Type = "Type";
        public static readonly string Table_Terms_Section = "Section";
        public static readonly string Table_Terms_Code = "Code";
        public static readonly string Table_Terms_Title = "Title";
        public static readonly string Table_Terms_ParentCode = "ParentCode";

        public static readonly string Table_AddCodes = "SearchIcd_AddCodes";

        public static readonly string Table_AddCodes_Id = "Id";
        public static readonly string Table_AddCodes_Type = "Type";
        public static readonly string Table_AddCodes_AddType = "AddType";
        public static readonly string Table_AddCodes_ParentCode = "ParentCode";
        public static readonly string Table_AddCodes_Code = "Code";

        public static readonly string Table_LinkedTitles = "SearchIcd_LinkedTitles";

        public static readonly string Table_LinkedTitles_Id = "Id";
        public static readonly string Table_LinkedTitles_Type = "Type";
        public static readonly string Table_LinkedTitles_Title = "Title";
        public static readonly string Table_LinkedTitles_Code = "Code";

        public static readonly string Table_Lists = "SearchIcd_Lists";

        public static readonly string Table_Lists_Id = "Id";
        public static readonly string Table_Lists_Type = "Type";
        public static readonly string Table_Lists_Data = "Data";
        public static readonly string Table_Lists_ListOrder = "ListOrder";
        public static readonly string Table_Lists_ListId = "ListId";
    }
}
