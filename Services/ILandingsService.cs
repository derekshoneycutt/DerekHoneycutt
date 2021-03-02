using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services
{
    /// <summary>
    /// Service for handling landings in the application
    /// </summary>
    public interface ILandingsService
    {
        /// <summary>
        /// Get all of the landings
        /// </summary>
        /// <param name="dbContext">DB Context to work with</param>
        /// <param name="log">Logging object to log operations</param>
        /// <returns>Collection of landings from the database</returns>
        Task<ICollection<BusinessModels.Landing>> GetAll();

        /// <summary>
        /// Get a specific landing by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get landing from</param>
        /// <param name="id">ID of the landing to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the landing</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.Landing> GetById(string id);
    }
}
