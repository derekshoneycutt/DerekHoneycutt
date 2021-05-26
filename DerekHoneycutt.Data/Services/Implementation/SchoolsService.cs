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
    /// Service for handling schools in the application
    /// </summary>
    public class SchoolsService : ISchoolsService
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext DbContext;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger Logger;

        public SchoolsService(
            DbModels.DatabaseContext dbContext,
            ILogger<SchoolsService> logger)
        {
            DbContext = dbContext;
            Logger = logger;
        }


        /// <summary>
        /// Parse a school from the database into business code
        /// </summary>
        /// <param name="s">School to parse</param>
        /// <returns>A business code representation of the school</returns>
        private static BusinessModels.School Parse(DbModels.School s)
        {
            return new BusinessModels.School()
            {
                Id = s.Id,
                Name = s.Name,
                City = s.City,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Program = s.Program,
                GPA = s.GPA,
                Other = s.Other,
                Order = s.Order
            };
        }

        /// <summary>
        /// Get the schools for a particular school wall page
        /// </summary>
        /// <param name="page">Page to get the school for (can include just SchoolsId, but must included that)</param>
        /// <returns>Collection of schools for the specified page</returns>
        public async Task<ICollection<BusinessModels.School>> GetFromPage(BusinessModels.SchoolsPage page)
        {
            ICollection<BusinessModels.School> ret;
            if (page.SchoolsPageOrigin != null)
            {
                Logger.LogInformation($"Getting schools from existing schools page origin");
                ret = (from school in page.SchoolsPageOrigin.Schools
                       orderby school.Order
                       select Parse(school)).ToList();
            }
            else
            {
                Logger.LogInformation($"Getting schools based on Schools Page Id");
                ret = await (from school in DbContext.Schools
                             where school.PageId == page.SchoolsId
                             orderby school.Order
                             select Parse(school)).ToListAsync();
            }

            return ret;
        }

        /// <summary>
        /// Get a specific school by its ID
        /// </summary>
        /// <param name="id">ID of the school to search for</param>
        /// <returns>Business object representing the school</returns>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public async Task<BusinessModels.School> GetById(Guid id)
        {
            Logger.LogInformation($"Getting first school with matching id {id}");
            var school = await DbContext.Schools.FirstOrDefaultAsync(s => id.Equals(s.Id));

            if (school == null)
            {
                Logger.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return Parse(school);
        }
    }
}
