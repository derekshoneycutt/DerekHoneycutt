using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services
{
    /// <summary>
    /// Email service definition for the application
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send an email from the application
        /// </summary>
        /// <param name="to">Address to send the email to</param>
        /// <param name="subject">Subject of the email to send</param>
        /// <param name="html">HTML Body of the email to send</param>
        /// <param name="from">Address of the sender for the email</param>
        Task Send(string to, string subject, string html, string from = null);
    }
}
