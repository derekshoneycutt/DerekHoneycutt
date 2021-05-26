using DerekHoneycutt.Data.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Controller for handling schools
    /// </summary>
    [Route("portfolio/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        /// <summary>
        /// Service for handling schools in the application
        /// </summary>
        private readonly ISchoolsService SchoolsService;

        public SchoolController(
            ISchoolsService schoolsService)
        {
            SchoolsService = schoolsService;
        }

        /// <summary>
        /// Translate a School business model to the REST model for the client
        /// </summary>
        /// <param name="school">Business model to translate</param>
        /// <returns>REST model to return to client</returns>
        public static RestModels.School TranslateSchool(Data.BusinessModels.School school)
        {
            return new RestModels.School()
            {
                Self = school.Id.ToString(),
                Name = school.Name,
                City = school.City,
                StartDate = school.StartDate,
                EndDate = school.EndDate,
                Gpa = school.GPA,
                Program = school.Program,
                Other = school.Other,
                Order = school.Order,
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/school/{school.Id}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// REST point for getting a particular school
        /// </summary>
        /// <param name="landingid">ID of the school to retrieve</param>
        /// <returns>Action result, hopefully including school</returns>
        [HttpGet("{schoolid}")]
        public async Task<IActionResult> GetSchool(Guid schoolid)
        {
            Data.BusinessModels.School school;

            try
            {
                school = await SchoolsService.GetById(schoolid);
            }
            catch (IndexOutOfRangeException iorex)
            {
                return new BadRequestObjectResult(new
                {
                    Error = "BadId",
                    iorex.Message
                });
            }
            catch (KeyNotFoundException knfex)
            {
                return new BadRequestObjectResult(new
                {
                    Error = "NotFound",
                    knfex.Message
                });
            }

            return new OkObjectResult(TranslateSchool(school));
        }
    }
}
