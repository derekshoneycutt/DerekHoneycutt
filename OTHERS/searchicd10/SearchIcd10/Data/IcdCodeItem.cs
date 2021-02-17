using ICD.DataAccess.ObjectModels;
using System.Collections.Generic;

namespace SearchIcd10.Data
{
    /// <summary>
    /// Wrapper class of the IcdCode class to include Comment, Enabled status, and Children
    /// </summary>
    public class IcdCodeItem
    {
        /// <summary>
        /// IcdCode object that is wrapped
        /// </summary>
        public IcdCode Code { get; set; }
        /// <summary>
        /// Comment to be added for the item
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Enabled status for the item
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Children for the item
        /// </summary>
        public IEnumerable<IcdCodeItem> Children { get; set; }
    }
}
