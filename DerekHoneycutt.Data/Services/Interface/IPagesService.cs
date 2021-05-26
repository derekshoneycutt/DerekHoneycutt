using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.Services.Interface
{
    /// <summary>
    /// Service for handling pages in the application
    /// </summary>
    public interface IPagesService
    {
        /// <summary>
        /// Get the pages for a particular landing
        /// </summary>
        /// <param name="landing">Landing to get the pages for (can include just Id, but must included that)</param>
        /// <returns>Collection of pages for the specified landing</returns>
        Task<ICollection<BusinessModels.Page>> GetFromLanding(BusinessModels.Landing landing);

        /// <summary>
        /// Get a specific page by its ID
        /// </summary>
        /// <param name="id">ID of the page to search for</param>
        /// <returns>Business object representing the page</returns>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.Page> GetById(Guid id);
    }
}
