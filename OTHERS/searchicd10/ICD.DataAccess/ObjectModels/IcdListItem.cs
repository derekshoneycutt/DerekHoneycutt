using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICD.DataAccess.ObjectModels
{
    /// <summary>
    /// A row from the ICD Lists table in the database
    /// </summary>
    public class IcdListItem
    {
        /// <summary>
        /// The row's primary key identifier
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// What type of item this row is : typically Code/Divider
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The Data associated to this row ; ICD-10 Code or Divider text
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Order identifier for this row
        /// Use to determine the order of items on a list
        /// </summary>
        public int ListOrder { get; set; }
        /// <summary>
        /// The List Identifier for this item
        /// </summary>
        public string ListId { get; set; }
    }
}
