using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services
{
    /// <summary>
    /// Services for handling resume experience jobs in the application
    /// </summary>
    public interface IResumeExpJobsService
    {
        /// <summary>
        /// Get a specific resume job by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get job from</param>
        /// <param name="id">ID of the job to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the job</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.ResumeExpJob> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log);
    }
}
