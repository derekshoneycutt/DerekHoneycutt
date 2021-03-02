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
        /// Get a specific resume job by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get job from</param>
        /// <param name="id">ID of the job to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the job</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public async Task<BusinessModels.ResumeExpJob> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log)
        {
            if (!Guid.TryParse(id, out Guid useGuid))
            {
                log.LogError("Invalid ID Passed, not appropriate Guid");
                throw new IndexOutOfRangeException();
            }

            var job = await dbContext.ResumeExpJobs.FirstOrDefaultAsync(j => useGuid.Equals(j.Id));

            if (job == null)
            {
                log.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return ParseJob(job);
        }
    }
}
