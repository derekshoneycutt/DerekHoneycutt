using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services.Core
{
    /// <summary>
    /// Service for handling images to show in the application
    /// </summary>
    public class ImagesService : IImagesService
    {
        /// <summary>
        /// Parse an image from the database into business code
        /// </summary>
        /// <param name="i">Image to parse</param>
        /// <returns>The business code representation</returns>
        private static BusinessModels.Image Parse(DbModels.Image i)
        {
            return new BusinessModels.Image()
            {
                Id = i.Id,
                Source = i.Source,
                Description = i.Description
            };
        }

        /// <summary>
        /// Get a specific image by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get job from</param>
        /// <param name="id">ID of the image to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the image</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public async Task<BusinessModels.Image> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log)
        {
            if (!Guid.TryParse(id, out Guid useGuid))
            {
                log.LogError("Invalid ID Passed, not appropriate Guid");
                throw new IndexOutOfRangeException();
            }

            var image = await dbContext.Images.FirstOrDefaultAsync(i => useGuid.Equals(i.Id));

            if (image == null)
            {
                log.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return Parse(image);

        }
    }
}
