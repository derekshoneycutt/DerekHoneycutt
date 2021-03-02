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
    /// Controller for handling portfolio pages
    /// </summary>
    [Route("portfolio/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        /// <summary>
        /// Service for handling pages in the application
        /// </summary>
        private readonly Services.IPagesService PagesService;

        public PageController(
            Services.IPagesService pagesService)
        {
            PagesService = pagesService;
        }

        /// <summary>
        /// Translate a Page business model into a REST model
        /// </summary>
        /// <param name="page">Business model to translate</param>
        /// <returns>REST model to return to the client</returns>
        public static RestModels.Page TranslatePage(BusinessModels.Page page)
        {
            RestModels.Page ret = null;

            if (page is BusinessModels.ImageWallPage iwpage)
            {
                ret = new RestModels.ImageWallPage()
                {
                    Description = iwpage.Description,
                    Images = iwpage.Images.Select(img => ImagesController.TranslateImage(img)).ToList(),
                    Type = BusinessModels.PageTypes.ImageWall
                };
            }
            else if (page is BusinessModels.ResumeExpPage repage)
            {
                ret = new RestModels.ResumeExpPage()
                {
                    Type = BusinessModels.PageTypes.ResumeExp,
                    Jobs = repage.Jobs.Select(rej => ResumeExpJobController.TranslateResumeExpJob(rej)).ToList()
                };
            }
            else if (page is BusinessModels.ResumeHeadPage rhpage)
            {
                ret = new RestModels.ResumeHeadPage()
                {
                    Type = BusinessModels.PageTypes.ResumeHead,
                    Description = rhpage.Description,
                    Competencies = rhpage.Competencies
                };
            }
            else if (page is BusinessModels.GitHubPage ghpage)
            {
                ret = new RestModels.GitHubPage()
                {
                    Type = BusinessModels.PageTypes.GitHub,
                    GitHub = ghpage.GitHub,
                    Description = ghpage.Description
                };
            }
            else if (page is BusinessModels.SchoolsPage spage)
            {
                ret = new RestModels.SchoolsPage()
                {
                    Type = BusinessModels.PageTypes.Schools,
                    Schools = spage.Schools.Select(school => SchoolController.TranslateSchool(school)).ToList()
                };
            }
            else if (page is BusinessModels.TextBlockPage tbpage)
            {
                ret = new RestModels.TextBlockPage()
                {
                    Type = BusinessModels.PageTypes.TextBlock,
                    Text = tbpage.Text
                };
            }

            if (ret != null)
            {
                ret.Self = page.Id.ToString();
                ret.Title = page.Title;
                ret.Subtitle = page.Subtitle;
                ret.Image = page.Image;
                ret.Background = page.Background;
                ret.Orientation = page.Orientation;
                ret.Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/page/{page.Id}",
                        Method= "GET",
                        PostData = null
                    }
                };
            }

            return ret;
        }

        /// <summary>
        /// REST point for getting a particular page
        /// </summary>
        /// <param name="pageid">ID of the page to retrieve</param>
        /// <returns>Action result, hopefully including page</returns>
        [HttpGet("{pageid}")]
        public async Task<IActionResult> GetPage(string pageid)
        {
            BusinessModels.Page page;

            try
            {
                page = await PagesService.GetById(pageid);
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

            return new OkObjectResult(TranslatePage(page));
        }
    }
}
