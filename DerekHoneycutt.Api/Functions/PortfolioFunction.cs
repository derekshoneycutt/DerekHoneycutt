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
    /// Function for retrieving the main portfolio "home" landing
    /// </summary>
    public class PortfolioFunction
    {
        /// <summary>
        /// Service for handling landings in the application
        /// </summary>
        private readonly ILandingsService LandingsService;
        /// <summary>
        /// JSON Parsing Options to utilize
        /// </summary>
        private readonly JsonSerializerOptions JsonOptions;

        public PortfolioFunction(
            ILandingsService landingsService,
            JsonSerializerOptions jsonOptions)
        {
            LandingsService = landingsService;
            JsonOptions = jsonOptions;
        }

        /// <summary>
        /// Entry point function to retrieve main portfolio home landing
        /// </summary>
        /// <param name="req">Request object</param>
        /// <param name="executionContext">Execution context of the current run</param>
        /// <returns>HTTP Response to send back to the client</returns>
        [Function("Portfolio")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(PortfolioFunction));
            logger.LogInformation("C# HTTP trigger function processed a Portfolio request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            try
            {
                logger.LogInformation($"Getting all landings");
                var landings = await LandingsService.GetAll();
                var stringBody = JsonSerializer.Serialize(new RestModels.Home()
                {
                    Landings = landings.Select(landing => LandingFunction.TranslateLanding(landing)).ToList(),
                    Links = new[]
                    {
                        new RestModels.Link()
                        {
                            Rel = "self",
                            Href = "api/portfolio",
                            Method = "GET",
                            PostData = null
                        }
                    }
                }, JsonOptions);
                response.WriteString(stringBody);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception attempting to retrieve and serialize portfolio: {ex.Message} STACK: {ex.StackTrace}");
                ResponseJsonHandler.SetExceptionToHttpResponse(ex, response, JsonOptions);
            }

            logger.LogInformation($"Sending response to client");

            return response;
        }
    }
}
