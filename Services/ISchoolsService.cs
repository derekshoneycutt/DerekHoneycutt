using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services
{
    /// <summary>
    /// Service for handling schools in the application
    /// </summary>
    public interface ISchoolsService
    {
        /// <summary>
        /// Get a specific school by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get school from</param>
        /// <param name="id">ID of the school to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the school</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.School> GetById(
               DbModels.DatabaseContext dbContext, string id, ILogger log);
    }
}
