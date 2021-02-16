using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbBusiness
{
    /// <summary>
    /// Static handlers for getting Landings data from the database into Business format
    /// </summary>
    public static class Landings
    {
        /// <summary>
        /// Get all of the landings
        /// </summary>
        /// <param name="dbContext">DB Context to work with</param>
        /// <param name="log">Logging object to log operations</param>
        /// <returns>Collection of landings from the database</returns>
        public static async Task<ICollection<BusinessModels.Landing>> GetAll(
            DbModels.DatabaseContext dbContext, ILogger log)
        {
            var models = await (from landing in dbContext.Landings

                                select new BusinessModels.Landing()
                                {
                                    Id = landing.Id,
                                    Href = landing.Href,
                                    Title = landing.Title,
                                    Subtitle = landing.Subtitle,
                                    Pages = (from page in landing.Pages
                                             orderby page.Order
                                             select 
                                                page.ImageWallExt != null ?
                                                    Pages.ParseImageWallPage(page, page.ImageWallExt) :
                                                page.ResumeExpExt != null ?
                                                    Pages.ParseResumeExpPage(page, page.ResumeExpExt) :
                                                page.ResumeHeadExt != null ?
                                                    Pages.ParseResumeHeadPage(page, page.ResumeHeadExt) :
                                                page.SchoolsExt != null ?
                                                    Pages.ParseSchoolsPage(page, page.SchoolsExt) :
                                                page.TextBlockExt != null ?
                                                    Pages.ParseTextBlockPage(page, page.TextBlockExt) :
                                                    Pages.ParseEmptyPage(page)).ToList()
                                }).ToListAsync();

            return models;
        }

        /// <summary>
        /// Get a specific landing by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get landing from</param>
        /// <param name="id">ID of the landing to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the landing</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public static async Task<BusinessModels.Landing> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log)
        {
            if (!Guid.TryParse(id, out Guid useGuid))
            {
                log.LogError("Invalid ID Passed, not appropriate Guid");
                throw new IndexOutOfRangeException();
            }

            var landing = await dbContext.Landings.FirstOrDefaultAsync(l => useGuid.Equals(l.Id));

            if (landing == null)
            {
                log.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return new BusinessModels.Landing()
            {
                Id = landing.Id,
                Href = landing.Href,
                Title = landing.Title,
                Subtitle = landing.Subtitle,
                Pages = Pages.ParsePages(landing.Pages).ToList()
            };
        }
    }
}
