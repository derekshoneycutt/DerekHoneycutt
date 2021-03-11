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
        /// Get the Resume experience jobs for a particular resume experience page
        /// </summary>
        /// <param name="page">Page to get the jobs for (can include just ImageWallId, but must included that)</param>
        /// <returns>Collection of jobs for the specified page</returns>
        Task<ICollection<BusinessModels.ResumeExpJob>> GetFromPage(BusinessModels.ResumeExpPage page);

        /// <summary>
        /// Get a specific resume job by its ID
        /// </summary>
        /// <param name="id">ID of the job to search for</param>
        /// <returns>Business object representing the job</returns>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.ResumeExpJob> GetById(Guid id);
    }
}
