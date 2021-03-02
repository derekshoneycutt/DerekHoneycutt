using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Controller for handling landings in the application
    /// </summary>
    [Route("portfolio/[controller]")]
    [ApiController]
    public class LandingController : ControllerBase
    {
        /// <summary>
        /// Service for handling landings in the application
        /// </summary>
        private readonly Services.ILandingsService LandingsService;

        public LandingController(
            Services.ILandingsService landingsService)
        {
            LandingsService = landingsService;
        }

        /// <summary>
        /// Translate a Landing business model into a REST model
        /// </summary>
        /// <param name="landing">Business model to translate</param>
        /// <returns>REST model to return to the client</returns>
        public static RestModels.Landing TranslateLanding(BusinessModels.Landing landing)
        {
            return new RestModels.Landing()
            {
                Self = landing.Id.ToString(),
                Href = landing.Href,
                Title = landing.Title,
                Subtitle = landing.Subtitle,
                Icon = landing.Icon,
                Order = landing.Order,
                Pages = landing.Pages.Select(p => PageController.TranslatePage(p)).ToList(),
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/landing/{landing.Id.ToString()}",
                        Method = "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// REST point for getting a particular landing
        /// </summary>
        /// <param name="landingid">ID of the landing to retrieve</param>
        /// <returns>Action result, hopefully including landing</returns>
        [HttpGet("{landingid}")]
        public async Task<IActionResult> GetLanding(string landingid)
        {
            BusinessModels.Landing landing;

            try
            {
                landing = await LandingsService.GetById(landingid);
            }
            catch (IndexOutOfRangeException iorex)
            {
                return new BadRequestObjectResult(new
                {
                    Error = "BadId",
                    iorex.Message
                });
            }
            catch (KeyNotFoundException knfex)
            {
                return new BadRequestObjectResult(new
                {
                    Error = "NotFound",
                    knfex.Message
                });
            }

            return new OkObjectResult(TranslateLanding(landing));
        }
    }
}
