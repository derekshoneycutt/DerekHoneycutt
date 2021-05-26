using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.Services.Interface
{
    /// <summary>
    /// Service for handling schools in the application
    /// </summary>
    public interface ISchoolsService
    {
        /// <summary>
        /// Get the schools for a particular school wall page
        /// </summary>
        /// <param name="page">Page to get the school for (can include just SchoolsId, but must included that)</param>
        /// <returns>Collection of schools for the specified page</returns>
        Task<ICollection<BusinessModels.School>> GetFromPage(BusinessModels.SchoolsPage page);

        /// <summary>
        /// Get a specific school by its ID
        /// </summary>
        /// <param name="id">ID of the school to search for</param>
        /// <returns>Business object representing the school</returns>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.School> GetById(Guid id);
    }
}
