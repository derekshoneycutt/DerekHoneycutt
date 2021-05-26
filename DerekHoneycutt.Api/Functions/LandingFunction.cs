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
    /// Function for retrieving a landing space for the application
    /// </summary>
    public class LandingFunction
    {
        /// <summary>
        /// Service for handling landings in the application
        /// </summary>
        private readonly ILandingsService LandingsService;
        /// <summary>
        /// JSON Parsing Options to utilize
        /// </summary>
        private readonly JsonSerializerOptions JsonOptions;

        public LandingFunction(
            ILandingsService landingsService,
            JsonSerializerOptions jsonOptions)
        {
            LandingsService = landingsService;
            JsonOptions = jsonOptions;
        }

        /// <summary>
        /// Translate a Landing business model into a REST model
        /// </summary>
        /// <param name="landing">Business model to translate</param>
        /// <returns>REST model to return to the client</returns>
        public static RestModels.Landing TranslateLanding(Data.BusinessModels.Landing landing)
        {
            return new RestModels.Landing()
            {
                Self = landing.Id.ToString(),
                Href = landing.Href,
                Title = landing.Title,
                Subtitle = landing.Subtitle,
                Icon = landing.Icon,
                Order = landing.Order,
                Pages = landing.Pages.Select(p => PageFunction.TranslatePage(p)).ToList(),
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"api/landing/{landing.Id}",
                        Method = "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// Entry point function to retrieve landings
        /// </summary>
        /// <param name="req">Request object</param>
        /// <param name="executionContext">Execution context of the current run</param>
        /// <param name="landingid">Landing ID parsed from URL</param>
        /// <returns>HTTP Response to send back to the client</returns>
        [Function("landing")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "landing/{landingid:Guid}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string landingid)
        {
            var logger = executionContext.GetLogger(nameof(LandingFunction));
            logger.LogInformation($"C# HTTP trigger function GET landing '{landingid}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            if (!Guid.TryParse(landingid, out var landingGuid))
            {
                logger.LogInformation($"Invalid GUID to landing, exiting with bad request");
                response.WriteString(JsonSerializer.Serialize(new
                {
                    Error = "BadId",
                    Message = $"Landing ID not in proper format: {landingid}"
                }, JsonOptions));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            // Once here, query the Schools service to do further work
            try
            {
                logger.LogInformation($"Getting landing {landingGuid}");
                var landing = await LandingsService.GetById(landingGuid);
                var stringBody = JsonSerializer.Serialize(TranslateLanding(landing), JsonOptions);
                response.WriteString(stringBody);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception attempting to retrieve and serialize landing: {ex.Message} STACK: {ex.StackTrace}");
                ResponseJsonHandler.SetExceptionToHttpResponse(ex, response, JsonOptions);
            }

            logger.LogInformation($"Sending response to client");

            return response;
        }
    }
}
