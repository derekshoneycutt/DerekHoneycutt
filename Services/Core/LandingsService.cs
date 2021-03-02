using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services.Core
{
    /// <summary>
    /// Service for handling landings in the application
    /// </summary>
    public class LandingsService : ILandingsService
    {
        /// <summary>
        /// Pages service for getting pages from each landing
        /// </summary>
        private readonly IPagesService PagesService;

        public LandingsService(IPagesService pagesService)
        {
            PagesService = pagesService;
        }

        /// <summary>
        /// Get all of the landings
        /// </summary>
        /// <param name="dbContext">DB Context to work with</param>
        /// <param name="log">Logging object to log operations</param>
        /// <returns>Collection of landings from the database</returns>
        public async Task<ICollection<BusinessModels.Landing>> GetAll(
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
                    Pages = PagesService.ParsePages(dbContext, pages, log).ToList()
                };

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
        public async Task<BusinessModels.Landing> GetById(
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
                Pages = PagesService.ParsePages(dbContext, landing.Pages, log).ToList()
            };
        }
    }
}
