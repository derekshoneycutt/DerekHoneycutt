using DerekHoneycutt.Data.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.Services.Implementation
{
    /// <summary>
    /// Service for handling images to show in the application
    /// </summary>
    public class ImagesService : IImagesService
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext DbContext;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger Logger;

        public ImagesService(
            DbModels.DatabaseContext dbContext,
            ILogger<ImagesService> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }


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
        /// Get the images for a particular image wall page
        /// </summary>
        /// <param name="page">Page to get the images for (can include just ImageWallId, but must included that)</param>
        /// <returns>Collection of images for the specified page</returns>
        public async Task<ICollection<BusinessModels.Image>> GetFromPage(BusinessModels.ImageWallPage page)
        {
            ICollection<BusinessModels.Image> ret;
            if (page.ImageWallPageOrigin != null)
            {
                ret = (from img in page.ImageWallPageOrigin.Images
                       orderby img.Order
                       select Parse(img)).ToList();
            }
            else
            {
                ret = await (from img in DbContext.Images
                             where img.PageId == page.ImageWallId
                             orderby img.Order
                             select Parse(img)).ToListAsync();
            }

            return ret;
        }

        /// <summary>
        /// Get a specific image by its ID
        /// </summary>
        /// <param name="id">ID of the image to search for</param>
        /// <returns>Business object representing the image</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public async Task<BusinessModels.Image> GetById(Guid id)
        {
            var image = await DbContext.Images.FirstOrDefaultAsync(i => id.Equals(i.Id));

            if (image == null)
            {
                Logger.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return Parse(image);

        }
    }
}
