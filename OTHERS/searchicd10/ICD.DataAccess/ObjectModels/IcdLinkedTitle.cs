using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess.ObjectModels
{
    /// <summary>
    /// A row from the table in the database used to describe alternative text for ICD-10 Codes
    /// </summary>
    public class IcdLinkedTitle
    {
        /// <summary>
        /// The row's primary key identifier
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// The type of Code this is associated to; Typically Diagnosis/Procedure
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The Alternative title to associate to an ICD-10 Code
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The ICD-10 Code to Associate the alternative title to
        /// </summary>
        public string Code { get; set; }
    }
}
