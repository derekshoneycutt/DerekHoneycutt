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
    /// Controller for handling images information
    /// </summary>
    [Route("portfolio/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext _dbContext;
        /// <summary>
        /// Service for handling images in the application
        /// </summary>
        private readonly Services.IImagesService ImagesService;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(
            DbModels.DatabaseContext dbContext,
            Services.IImagesService imagesService,
            ILogger<ImagesController> logger)
        {
            _dbContext = dbContext;
            ImagesService = imagesService;
            _logger = logger;
        }

        /// <summary>
        /// Translate an Image from the business model to a REST return model
        /// </summary>
        /// <param name="img">Business model to translate</param>
        /// <returns>REST object to return to the client</returns>
        public static RestModels.Image TranslateImage(BusinessModels.Image img)
        {
            return new RestModels.Image()
            {
                Self = img.Id.ToString(),
                Source = img.Source,
                Description = img.Description,
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/images/{img.Id.ToString()}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// REST point for getting a particular Image handle
        /// </summary>
        /// <param name="imgid">ID of the image to retrieve</param>
        /// <returns>Action result, hopefully including job</returns>
        [HttpGet("{imgid}")]
        public async Task<IActionResult> GetImage(string imgid)
        {
            BusinessModels.Image img;

            try
            {
                img = await ImagesService.GetById(_dbContext, imgid, _logger);
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

            return new OkObjectResult(TranslateImage(img));
        }
    }
}
