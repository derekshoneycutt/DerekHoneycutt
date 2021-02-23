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
            var dbModels = await (from landing in dbContext.Landings
                                  orderby landing.Order
                                  select landing).ToListAsync();

            var ret = new List<BusinessModels.Landing>(dbModels.Count);

            foreach (var landing in dbModels)
            {
                var pages = (from page in landing.Pages
                             orderby page.Order
                             select page).ToList();

                var newLanding = new BusinessModels.Landing()
                {
                    Id = landing.Id,
                    Href = landing.Href,
                    Title = landing.Title,
                    Subtitle = landing.Subtitle,
                    Icon = landing.Icon,
                    Order = landing.Order,
                    Pages = new List<BusinessModels.Page>(pages.Count)
                };

                foreach (var page in pages)
                {
                    newLanding.Pages.Add(Pages.ParsePage(page));
                }

                ret.Add(newLanding);
            }

            return ret;
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
                Pages = Pages.ParsePages(dbContext, landing.Pages, log).ToList()
            };
        }
    }
}
