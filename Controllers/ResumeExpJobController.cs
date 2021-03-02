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
    /// Controller for accessing resume experience job information
    /// </summary>
    [Route("portfolio/[controller]")]
    [ApiController]
    public class ResumeExpJobController : ControllerBase
    {
        /// <summary>
        /// Service for handling resume experience jobs in the application
        /// </summary>
        private readonly Services.IResumeExpJobsService ResumeExpJobsService;

        public ResumeExpJobController(
            Services.IResumeExpJobsService resumeExpJobsService)
        {
            ResumeExpJobsService = resumeExpJobsService;
        }

        /// <summary>
        /// Translate a ResumeExpJob from the business model to a REST return model
        /// </summary>
        /// <param name="rej">Business model to translate</param>
        /// <returns>REST object to return to the client</returns>
        public static RestModels.ResumeExpJob TranslateResumeExpJob(BusinessModels.ResumeExpJob rej)
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
                        Href = $"portfolio/resumeexpjob/{rej.Id.ToString()}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// REST point for getting a particular ResumeExp Job
        /// </summary>
        /// <param name="jobid">ID of the job to retrieve</param>
        /// <returns>Action result, hopefully including job</returns>
        [HttpGet("{jobid}")]
        public async Task<IActionResult> GetResumeExpJob(string jobid)
        {
            BusinessModels.ResumeExpJob job;

            try
            {
                job = await ResumeExpJobsService.GetById(jobid);
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

            return new OkObjectResult(TranslateResumeExpJob(job));
        }
    }
}
