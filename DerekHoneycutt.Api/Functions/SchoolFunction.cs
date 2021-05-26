using System;
using System.Collections.Generic;
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
    /// Function for retrieving a single school for the application
    /// </summary>
    public class SchoolFunction
    {
        /// <summary>
        /// Service for handling schools in the application
        /// </summary>
        private readonly ISchoolsService SchoolsService;
        /// <summary>
        /// JSON Parsing Options to utilize
        /// </summary>
        private readonly JsonSerializerOptions JsonOptions;

        public SchoolFunction(
            ISchoolsService schoolsService,
            JsonSerializerOptions jsonOptions)
        {
            SchoolsService = schoolsService;
            JsonOptions = jsonOptions;
        }

        /// <summary>
        /// Translate a School business model to the REST model for the client
        /// </summary>
        /// <param name="school">Business model to translate</param>
        /// <returns>REST model to return to client</returns>
        public static RestModels.School TranslateSchool(Data.BusinessModels.School school)
        {
            return new RestModels.School()
            {
                Self = school.Id.ToString(),
                Name = school.Name,
                City = school.City,
                StartDate = school.StartDate,
                EndDate = school.EndDate,
                Gpa = school.GPA,
                Program = school.Program,
                Other = school.Other,
                Order = school.Order,
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"api/school/{school.Id}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// Entry point function to retrieve schools
        /// </summary>
        /// <param name="req">Request object</param>
        /// <param name="executionContext">Execution context of the current run</param>
        /// <param name="schoolid">School ID parsed from URL</param>
        /// <returns>HTTP Response to send back to the client</returns>
        [Function("school")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "school/{schoolid:Guid}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string schoolid)
        {
            var logger = executionContext.GetLogger(nameof(SchoolFunction));
            logger.LogInformation($"C# HTTP trigger function GET school '{schoolid}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            if (!Guid.TryParse(schoolid, out var schoolGuid))
            {
                logger.LogInformation($"Invalid GUID to school, exiting with bad request");
                response.WriteString(JsonSerializer.Serialize(new
                {
                    Error = "BadId",
                    Message = $"School ID not in proper format: {schoolid}"
                }, JsonOptions));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            // Once here, query the Schools service to do further work
            try
            {
                logger.LogInformation($"Getting school {schoolGuid}");
                var school = await SchoolsService.GetById(schoolGuid);
                var stringBody = JsonSerializer.Serialize(TranslateSchool(school), JsonOptions);
                response.WriteString(stringBody);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception attempting to retrieve and serialize school: {ex.Message} STACK: {ex.StackTrace}");
                ResponseJsonHandler.SetExceptionToHttpResponse(ex, response, JsonOptions);
            }

            logger.LogInformation($"Sending response to client");

            return response;
        }
    }
}
