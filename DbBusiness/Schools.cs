using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbBusiness
{
    /// <summary>
    /// Methods for converting schools from the database to business code representation
    /// </summary>
    public static class Schools
    {
        /// <summary>
        /// Parse a school from the database into business code
        /// </summary>
        /// <param name="s">School to parse</param>
        /// <returns>A business code representation of the school</returns>
        public static BusinessModels.School Parse(DbModels.School s)
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
        /// Get a specific school by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get school from</param>
        /// <param name="id">ID of the school to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the school</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public static async Task<BusinessModels.School> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log)
        {
            if (!Guid.TryParse(id, out Guid useGuid))
            {
                log.LogError("Invalid ID Passed, not appropriate Guid");
                throw new IndexOutOfRangeException();
            }

            var school = await dbContext.Schools.FirstOrDefaultAsync(s => useGuid.Equals(s.Id));

            if (school == null)
            {
                log.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return Parse(school);
        }
    }
}
