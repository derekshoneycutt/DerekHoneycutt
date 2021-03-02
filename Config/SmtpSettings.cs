using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Config
{
    /// <summary>
    /// Defines a block of SMTP Connection settings
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Gets or Sets the Server address
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// Gets or Sets the Port the server runs on
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Gets or Sets a username to login to the server with
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or Sets a password to login to the server with
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or Sets an email address to mark the email as from
        /// </summary>
        public string EmailFrom { get; set; }
    }
}
