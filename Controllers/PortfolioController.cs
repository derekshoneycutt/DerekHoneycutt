using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Defines the primary controller for the portfolio methods of the application
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : Controller
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext _dbContext;
        /// <summary>
        /// Service for handling landings in the application
        /// </summary>
        private readonly Services.ILandingsService LandingsService;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(
            DbModels.DatabaseContext dbContext,
            Services.ILandingsService landingsService,
            ILogger<PortfolioController> logger)
        {
            _dbContext = dbContext;
            LandingsService = landingsService;
            _logger = logger;
        }

        /// <summary>
        /// Initial entry point for the portfolio application
        /// </summary>
        /// <returns>Action result., hopefully containing all needed info for application to continue</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var landings = await LandingsService.GetAll(_dbContext, _logger);

            return new OkObjectResult(new RestModels.Home()
            {
                Landings = landings.Select(landing => LandingController.TranslateLanding(landing)).ToList(),
                Links = new []
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = "portfolio",
                        Method = "GET",
                        PostData = null
                    }/*,  // Keep this, just in case? IDK.
                    new RestModels.Link()
                    {
                        Rel = "Contact",
                        Href = "portfolio/contact",
                        Method = "POST",
                        PostData = new RequestModels.PostContact()
                        {
                            From = "Name",
                            Return = "person@example.com",
                            Message = "Any plain text to send"
                        }
                    }*/
                }
            });
        }
    }
}