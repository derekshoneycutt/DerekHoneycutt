using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services
{
    /// <summary>
    /// Service for handling images to show in the application
    /// </summary>
    public interface IImagesService
    {
        /// <summary>
        /// Get a specific image by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get job from</param>
        /// <param name="id">ID of the image to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the image</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.Image> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log);
    }
}
