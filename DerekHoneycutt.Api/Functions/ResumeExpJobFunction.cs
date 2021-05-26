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
    /// Function for retrieving single resume experience jobs for the application
    /// </summary>
    public class ResumeExpJobFunction
    {
        /// <summary>
        /// Service for handling resume experience jobs in the application
        /// </summary>
        private readonly IResumeExpJobsService ResumeExpJobsService;
        /// <summary>
        /// JSON Parsing Options to utilize
        /// </summary>
        private readonly JsonSerializerOptions JsonOptions;

        public ResumeExpJobFunction(
            IResumeExpJobsService resumeExpJobsService,
            JsonSerializerOptions jsonOptions)
        {
            ResumeExpJobsService = resumeExpJobsService;
            JsonOptions = jsonOptions;
        }

        /// <summary>
        /// Translate a ResumeExpJob from the business model to a REST return model
        /// </summary>
        /// <param name="rej">Business model to translate</param>
        /// <returns>REST object to return to the client</returns>
        public static RestModels.ResumeExpJob TranslateResumeExpJob(Data.BusinessModels.ResumeExpJob rej)
        {
            return new RestModels.ResumeExpJob()
            {
                Self = rej.Id.ToString(),
                Title = rej.Title,
                Employer = rej.Employer,
                EmployerCity = rej.EmployerCity,
                StartDate = rej.StartDate,
                EndDate = rej.EndDate,
                Description = rej.Description,
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"api/resumeexpjob/{rej.Id}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// Entry point function to retrieve Resume Experience Jobs
        /// </summary>
        /// <param name="req">Request object</param>
        /// <param name="executionContext">Execution context of the current run</param>
        /// <param name="jobid">Job ID parsed from URL</param>
        /// <returns>HTTP Response to send back to the client</returns>
        [Function("resumeexpjob")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "resumeexpjob/{jobid:Guid}")]
            HttpRequestData req,
            FunctionContext executionContext,
            string jobid)
        {
            var logger = executionContext.GetLogger(nameof(ResumeExpJobFunction));
            logger.LogInformation($"C# HTTP trigger function GET Resume Exp Job '{jobid}'");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            if (!Guid.TryParse(jobid, out var jobGuid))
            {
                logger.LogInformation($"Invalid GUID to job, exiting with bad request");
                response.WriteString(JsonSerializer.Serialize(new
                {
                    Error = "BadId",
                    Message = $"Job ID not in proper format: {jobid}"
                }, JsonOptions));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            // Once here, query the Schools service to do further work
            try
            {
                logger.LogInformation($"Getting job {jobGuid}");
                var job = await ResumeExpJobsService.GetById(jobGuid);
                var stringBody = JsonSerializer.Serialize(TranslateResumeExpJob(job), JsonOptions);
                response.WriteString(stringBody);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception attempting to retrieve and serialize job: {ex.Message} STACK: {ex.StackTrace}");
                ResponseJsonHandler.SetExceptionToHttpResponse(ex, response, JsonOptions);
            }

            logger.LogInformation($"Sending response to client");

            return response;
        }
    }
}
