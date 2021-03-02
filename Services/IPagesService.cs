using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services
{
    /// <summary>
    /// Service for handling pages in the application
    /// </summary>
    public interface IPagesService
    {
        /// <summary>
        /// Parse a Page from the database into Business models
        /// </summary>
        /// <param name="inpages">The page to translate</param>
        /// <returns>Business model representing the page</returns>
        IEnumerable<BusinessModels.Page> ParsePages(
            DbModels.DatabaseContext dbContext, IEnumerable<DbModels.Page> inpages, ILogger log);

        /// <summary>
        /// Get a specific page by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get page from</param>
        /// <param name="id">ID of the page to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the page</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.Page> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log);
    }
}
