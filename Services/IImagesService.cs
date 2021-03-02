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
        /// Get the images for a particular image wall page
        /// </summary>
        /// <param name="page">Page to get the images for (can include just ImageWallId, but must included that)</param>
        /// <returns>Collection of images for the specified page</returns>
        Task<ICollection<BusinessModels.Image>> GetFromPage(BusinessModels.ImageWallPage page);

        /// <summary>
        /// Get a specific image by its ID
        /// </summary>
        /// <param name="id">ID of the image to search for</param>
        /// <returns>Business object representing the image</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        Task<BusinessModels.Image> GetById(string id);
    }
}
