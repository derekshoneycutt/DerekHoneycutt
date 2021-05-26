using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DerekHoneycutt.Data.Services.Interface;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DerekHoneycutt.Api
{
    /// <summary>
    /// Function for handling 
    /// </summary>
    public class ImagesFunction
    {
        /// <summary>
        /// Service for handling images in the application
        /// </summary>
        private readonly IImagesService ImagesService;
        /// <summary>
        /// JSON Parsing Options to utilize
        /// </summary>
        private readonly JsonSerializerOptions JsonOptions;

        public ImagesFunction(
            IImagesService imagesService,
            JsonSerializerOptions jsonOptions)
        {
            ImagesService = imagesService;
            JsonOptions = jsonOptions;
        }

        /// <summary>
        /// Translate an Image from the business model to a REST return model
        /// </summary>
        /// <param name="img">Business model to translate</param>
        /// <returns>REST object to return to the client</returns>
        public static RestModels.Image TranslateImage(Data.BusinessModels.Image img)
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
                        Href = $"api/images/{img.Id}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// Entry point function to retrieve images
        /// </summary>
        /// <param name="req">Request object</param>
        /// <param name="executionContext">Execution context of the current run</param>
        /// <param name="imgid">Image ID parsed from URL</param>
        /// <returns>HTTP Response to send back to the client</returns>
        [Function("images")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images/{imgid:Guid}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string imgid)
        {
            var logger = executionContext.GetLogger(nameof(ImagesFunction));
            logger.LogInformation($"C# HTTP trigger function GET images '{imgid}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            if (!Guid.TryParse(imgid, out var imgGuid))
            {
                logger.LogInformation($"Invalid GUID to images, exiting with bad request");
                response.WriteString(JsonSerializer.Serialize(new
                {
                    Error = "BadId",
                    Message = $"Image ID not in proper format: {imgid}"
                }, JsonOptions));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            // Once here, query the Images service to do further work
            try
            {
                logger.LogInformation($"Getting image {imgGuid}");
                var img = await ImagesService.GetById(imgGuid);
                var stringBody = JsonSerializer.Serialize(TranslateImage(img), JsonOptions);
                response.WriteString(stringBody);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception attempting to retrieve and serialize img: {ex.Message} STACK: {ex.StackTrace}");
                ResponseJsonHandler.SetExceptionToHttpResponse(ex, response, JsonOptions);
            }

            logger.LogInformation($"Sending response to client");

            return response;
        }
    }
}
