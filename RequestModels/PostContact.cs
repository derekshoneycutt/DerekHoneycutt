using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.RequestModels
{
    /// <summary>
    /// Defines a form to post contact requests to the server
    /// </summary>
    public class PostContact
    {
        /// <summary>
        /// Gets or Sets the name of the person requesting contact
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Gets or Sets the return email of the person requestion contact
        /// </summary>
        public string Return { get; set; }
        /// <summary>
        /// Gets or Sets the message of contact the user wishes to send
        /// </summary>
        public string Message { get; set; }
    }
}
