using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess.ObjectModels
{
    /// <summary>
    /// A row in the Database to describe Additional codes that act as children of another code
    /// </summary>
    public class IcdAddCode
    {
        /// <summary>
        /// The row's primary key identifier
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// The type of codes associated; Typically Diagnosis/Procedure
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The Type of Additional Code; Typically CodeFirst/CodeAdditional/CodeAlso
        /// </summary>
        public string AddType { get; set; }
        /// <summary>
        /// The Parent code to associate the Additional Code to
        /// </summary>
        public string ParentCode { get; set; }
        /// <summary>
        /// The Additional code to associate as a child to the ParentCode
        /// </summary>
        public string Code { get; set; }
    }
}
