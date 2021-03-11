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
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext DbContext;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger Logger;

        /// <summary>
        /// Pages service for getting pages from each landing
        /// </summary>
        private readonly IPagesService PagesService;

        public LandingsService(
            DbModels.DatabaseContext dbContext,
            ILogger<SchoolsService> logger,
            IPagesService pagesService)
        {
            DbContext = dbContext;
            Logger = logger;
            PagesService = pagesService;
        }

        /// <summary>
        /// Get all of the landings
        /// </summary>
        /// <param name="dbContext">DB Context to work with</param>
        /// <param name="log">Logging object to log operations</param>
        /// <returns>Collection of landings from the database</returns>
        public async Task<ICollection<BusinessModels.Landing>> GetAll()
        {
            var dbModels = await (from landing in DbContext.Landings
                                  orderby landing.Order
                                  select landing).ToListAsync();

            var ret = new List<BusinessModels.Landing>(dbModels.Count);

            foreach (var landing in dbModels)
            {
                var newLanding = new BusinessModels.Landing()
                {
                    LandingOrigin = landing,
                    Id = landing.Id,
                    Href = landing.Href,
                    Title = landing.Title,
                    Subtitle = landing.Subtitle,
                    Icon = landing.Icon,
                    Order = landing.Order,
                };
                newLanding.Pages = await PagesService.GetFromLanding(newLanding);

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
        public async Task<BusinessModels.Landing> GetById(Guid id)
        {
            var landing = await DbContext.Landings.FirstOrDefaultAsync(l => id.Equals(l.Id));

            if (landing == null)
            {
                Logger.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            var retLanding = new BusinessModels.Landing()
            {
                LandingOrigin = landing,
                Id = landing.Id,
                Href = landing.Href,
                Title = landing.Title,
                Subtitle = landing.Subtitle
            };
            retLanding.Pages = await PagesService.GetFromLanding(retLanding);
            return retLanding;
        }
    }
}
