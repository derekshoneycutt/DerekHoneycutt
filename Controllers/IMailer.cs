using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Defines an interface for sending emails
    /// </summary>
    public interface IMailer
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="sendto">Email address to send to</param>
        /// <param name="data">Contact Form to send for email</param>
        /// <returns>Task for async operation</returns>
        Task SendEmailAsync(string sendto, RestModels.PostContact data);
    }
}
