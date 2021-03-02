using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Controller for handling contact attempts (are we keeping this?)
    /// </summary>
    [Route("portfolio/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext _dbContext;
        /// <summary>
        /// Mailer to use if the user is trying to contact me
        /// </summary>
        private readonly Services.IEmailService _mailer;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger<ContactController> _logger;
        /// <summary>
        /// Configuration object to retrieve
        /// </summary>
        private readonly IConfiguration Configuration;

        public ContactController(
            DbModels.DatabaseContext dbContext,
            Services.IEmailService mailer,
            ILogger<ContactController> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mailer = mailer;
            _logger = logger;
            Configuration = configuration;
        }



        /// <summary>
        /// POST access point for contacting the webmaster
        /// </summary>
        /// <param name="form">Contact information, including who is sending the message and the message</param>
        /// <returns>Action result describing success or failure object</returns>
        [HttpPost]
        public async Task<IActionResult> PostContact(RestModels.PostContact form)
        {
            return new OkObjectResult(new { Message = "Message not actually sent." });

            /*var sendto = Configuration.GetValue("ContactEmail", "derekhoneycutthole@mailinator.com");

            //
            /*await _mailer.Send(
                sendto, 
                $"[Portfolio Contact] {form.From}",
                $"Message received from {form.From}: {form.Return}\n\n============\n\n{form.Message}",
                form.From);* /

            return new OkObjectResult(new { Message = "Sent Successful!" });*/
        }
    }
}
