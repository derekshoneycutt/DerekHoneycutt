using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DerekHoneycutt.Data.Services.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DerekHoneycutt.Api.Functions
{
    /// <summary>
    /// Function class to retrieve a single page for the application
    /// </summary>
    public class PageFunction
    {
        /// <summary>
        /// Service for handling pages in the application
        /// </summary>
        private readonly IPagesService PagesService;
        /// <summary>
        /// JSON Parsing Options to utilize
        /// </summary>
        private readonly JsonSerializerOptions JsonOptions;

        public PageFunction(
            IPagesService pagesService,
            JsonSerializerOptions jsonOptions)
        {
            PagesService = pagesService;
            JsonOptions = jsonOptions;
        }

        /// <summary>
        /// Translate a Page business model into a REST model
        /// </summary>
        /// <param name="page">Business model to translate</param>
        /// <returns>REST model to return to the client</returns>
        public static RestModels.Page TranslatePage(Data.BusinessModels.Page page)
        {
            RestModels.Page ret = null;

            if (page is Data.BusinessModels.ImageWallPage iwpage)
            {
                ret = new RestModels.ImageWallPage()
                {
                    Description = iwpage.Description,
                    Images = iwpage.Images.Select(img => ImagesFunction.TranslateImage(img)).ToList(),
                    Type = Data.BusinessModels.PageTypes.ImageWall
                };
            }
            else if (page is Data.BusinessModels.ResumeExpPage repage)
            {
                ret = new RestModels.ResumeExpPage()
                {
                    Type = Data.BusinessModels.PageTypes.ResumeExp,
                    Jobs = repage.Jobs.Select(rej => ResumeExpJobFunction.TranslateResumeExpJob(rej)).ToList()
                };
            }
            else if (page is Data.BusinessModels.ResumeHeadPage rhpage)
            {
                ret = new RestModels.ResumeHeadPage()
                {
                    Type = Data.BusinessModels.PageTypes.ResumeHead,
                    Description = rhpage.Description,
                    Competencies = rhpage.Competencies
                };
            }
            else if (page is Data.BusinessModels.GitHubPage ghpage)
            {
                ret = new RestModels.GitHubPage()
                {
                    Type = Data.BusinessModels.PageTypes.GitHub,
                    GitHub = ghpage.GitHub,
                    Description = ghpage.Description
                };
            }
            else if (page is Data.BusinessModels.SchoolsPage spage)
            {
                ret = new RestModels.SchoolsPage()
                {
                    Type = Data.BusinessModels.PageTypes.Schools,
                    Schools = spage.Schools.Select(school => SchoolFunction.TranslateSchool(school)).ToList()
                };
            }
            else if (page is Data.BusinessModels.TextBlockPage tbpage)
            {
                ret = new RestModels.TextBlockPage()
                {
                    Type = Data.BusinessModels.PageTypes.TextBlock,
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
                        Href = $"api/page/{page.Id}",
                        Method= "GET",
                        PostData = null
                    }
                };
            }

            return ret;
        }

        /// <summary>
        /// Entry point function to retrieve pages
        /// </summary>
        /// <param name="req">Request object</param>
        /// <param name="executionContext">Execution context of the current run</param>
        /// <param name="pageid">Page ID parsed from URL</param>
        /// <returns>HTTP Response to send back to the client</returns>
        [Function("page")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "page/{pageid:Guid}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string pageid)
        {
            var logger = executionContext.GetLogger(nameof(PageFunction));
            logger.LogInformation($"C# HTTP trigger function GET Page '{pageid}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            if (!Guid.TryParse(pageid, out var pageGuid))
            {
                logger.LogInformation($"Invalid GUID to page, exiting with bad request");
                response.WriteString(JsonSerializer.Serialize(new
                {
                    Error = "BadId",
                    Message = $"Page ID not in proper format: {pageid}"
                }, JsonOptions));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            // Once here, query the Schools service to do further work
            try
            {
                logger.LogInformation($"Getting page {pageGuid}");
                var page = await PagesService.GetById(pageGuid);
                var stringBody = JsonSerializer.Serialize(TranslatePage(page), JsonOptions);
                response.WriteString(stringBody);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception attempting to retrieve and serialize page: {ex.Message} STACK: {ex.StackTrace}");
                ResponseJsonHandler.SetExceptionToHttpResponse(ex, response, JsonOptions);
            }

            logger.LogInformation($"Sending response to client");

            return response;
        }
    }
}
