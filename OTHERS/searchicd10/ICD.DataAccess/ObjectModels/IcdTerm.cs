using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess.ObjectModels
{
    /// <summary>
    /// A Row of the ICD Terms table in the database
    /// </summary>
    public class IcdTerm
    {
        /// <summary>
        /// The row's primary key identifier
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// The type of the code, typically Diagnosis/Procedure
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// What section of codes this belongs to (Entered to completeness and possible future use)
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// The ICD-10 Code for this row
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The ICD-10 Text for the ICD-10 Code
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The ICD-10 Parent code of this code -- may be null if no parents
        /// </summary>
        public string ParentCode { get; set; }
    }
}
