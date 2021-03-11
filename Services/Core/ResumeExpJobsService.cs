using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services.Core
{
    /// <summary>
    /// Services for handling resume experience jobs in the application
    /// </summary>
    public class ResumeExpJobsService : IResumeExpJobsService
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext DbContext;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger Logger;

        public ResumeExpJobsService(
            DbModels.DatabaseContext dbContext,
            ILogger<ResumeExpJobsService> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }


        /// <summary>
        /// Parse a job from the database into business code
        /// </summary>
        /// <param name="j">Job to parse</param>
        /// <returns>The business code representation</returns>
        private static BusinessModels.ResumeExpJob ParseJob(DbModels.ResumeExpJob j)
        {
            return new BusinessModels.ResumeExpJob()
            {
                Id = j.Id,
                Title = j.Title,
                Employer = j.Employer,
                EmployerCity = j.EmployerCity,
                StartDate = j.StartDate,
                EndDate = j.EndDate,
                Description = j.Description
            };
        }

        /// <summary>
        /// Get the Resume experience jobs for a particular resume experience page
        /// </summary>
        /// <param name="page">Page to get the jobs for (can include just ImageWallId, but must included that)</param>
        /// <returns>Collection of jobs for the specified page</returns>
        public async Task<ICollection<BusinessModels.ResumeExpJob>> GetFromPage(BusinessModels.ResumeExpPage page)
        {
            ICollection<BusinessModels.ResumeExpJob> ret;
            if (page.ResumeExpPageOrigin != null)
            {
                ret = (from job in page.ResumeExpPageOrigin.Jobs
                       select ParseJob(job)).ToList();
            }
            else
            {
                ret = await (from job in DbContext.ResumeExpJobs
                             where job.PageId == page.ResumeExpId
                             select ParseJob(job)).ToListAsync();
            }

            return ret;
        }

        /// <summary>
        /// Get a specific resume job by its ID
        /// </summary>
        /// <param name="id">ID of the job to search for</param>
        /// <returns>Business object representing the job</returns>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public async Task<BusinessModels.ResumeExpJob> GetById(Guid id)
        {
            var job = await DbContext.ResumeExpJobs.FirstOrDefaultAsync(j => id.Equals(j.Id));

            if (job == null)
            {
                Logger.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return ParseJob(job);
        }
    }
}
