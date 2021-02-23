using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.DbBusiness
{
    /// <summary>
    /// Methods for converting pages from the database to business code representation
    /// </summary>
    public static class Pages
    {
        /// <summary>
        /// Parse an empty page
        /// </summary>
        /// <param name="page">empty page to parse</param>
        /// <returns>A parsed, useless page</returns>
        public static BusinessModels.Page ParseEmptyPage(DbModels.Page page)
        {
            return new BusinessModels.Page()
            {
                Id = page.Id,
                Type = String.Empty,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation
            };
        }

        /// <summary>
        /// Parse an Image Wall page
        /// </summary>
        /// <param name="page">Image wall page to parse</param>
        /// <returns>A parsed Image Wall page</returns>
        public static BusinessModels.ImageWallPage ParseImageWallPage(
            DbModels.Page page)
        {
            var images = (from image in page.ImageWallExt.Images
                          orderby image.Order
                          select image).ToList();
            return new BusinessModels.ImageWallPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ImageWall,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                ImageWallId = page.ImageWallExt.Id,
                Description = page.ImageWallExt.Description,
                Images = (from image in images
                          select new BusinessModels.Image()
                          {
                              Id = image.Id,
                              Source = image.Source,
                              Description = image.Description
                          }).ToList()
            };
        }

        /// <summary>
        /// Parse a Resume Experience page
        /// </summary>
        /// <param name="page">Resume Experience page to parse</param>
        /// <returns>A parsed Resume Experience page</returns>
        public static BusinessModels.ResumeExpPage ParseResumeExpPage(
            DbModels.Page page)
        {
            var jobs = (from job in page.ResumeExpExt.Jobs
                        select job).ToList();
            return new BusinessModels.ResumeExpPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeExp,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                ResumeExpId = page.ResumeExpExt.Id,
                Jobs = (from job in jobs
                        select new BusinessModels.ResumeExpJob()
                        {
                            Id = job.Id,
                            Title = job.Title,
                            Description = job.Description,
                            Employer = job.Employer,
                            EmployerCity = job.EmployerCity,
                            StartDate = job.StartDate,
                            EndDate = job.EndDate
                        }).ToList()
            };
        }

        /// <summary>
        /// Parse a Resume Head page
        /// </summary>
        /// <param name="page">Resume Head page to parse</param>
        /// <returns>A parsed Resume Head page</returns>
        public static BusinessModels.ResumeHeadPage ParseResumeHeadPage(
            DbModels.Page page)
        {
            return new BusinessModels.ResumeHeadPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeHead,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                ResumeHeadId = page.ResumeHeadExt.Id,
                Description = page.ResumeHeadExt.Description,
                Competencies = page.ResumeHeadExt.Competencies
            };
        }

        /// <summary>
        /// Parse a Resume GitHub page
        /// </summary>
        /// <param name="page">Resume GitHub page to parse</param>
        /// <returns>A parsed Resume GitHub page</returns>
        public static BusinessModels.GitHubPage ParseGitHubPage(
            DbModels.Page page)
        {
            return new BusinessModels.GitHubPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.GitHub,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                GitHubId = page.GitHubPageExt.Id,
                Description = page.GitHubPageExt.Description,
                GitHub = page.GitHubPageExt.GitHub
            };
        }

        /// <summary>
        /// Parse a Schools page
        /// </summary>
        /// <param name="page">Schools page to parse</param>
        /// <returns>A parsed Schools page</returns>
        public static BusinessModels.SchoolsPage ParseSchoolsPage(
            DbModels.Page page)
        {
            var schools = (from school in page.SchoolsExt.Schools
                           select school).ToList();
            return new BusinessModels.SchoolsPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeExp,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                SchoolsId = page.SchoolsExt.Id,
                Schools = (from school in schools
                           orderby school.Order
                           select new BusinessModels.School()
                           {
                               Id = school.Id,
                               Name = school.Name,
                               City = school.City,
                               Program = school.Program,
                               StartDate = school.StartDate,
                               EndDate = school.EndDate,
                               GPA = school.GPA,
                               Other = school.Other,
                               Order = school.Order
                           }).ToList()
            };
        }

        /// <summary>
        /// Parse a Text Block page
        /// </summary>
        /// <param name="page">Text Block page to parse</param>
        /// <returns>A parsed Text Block page</returns>
        public static BusinessModels.TextBlockPage ParseTextBlockPage(
            DbModels.Page page)
        {
            return new BusinessModels.TextBlockPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.TextBlock,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                TextBlockId = page.TextBlockExt.Id,
                Text = page.TextBlockExt.Text
            };
        }

        /// <summary>
        /// Parse a page
        /// </summary>
        /// <param name="page">Page to parse</param>
        /// <returns>A parsed page</returns>
        public static BusinessModels.Page ParsePage(
            DbModels.Page page)
        {
            if (String.Equals(page.Type, BusinessModels.PageTypes.ResumeHead))
            {
                return ParseResumeHeadPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.ResumeExp))
            {
                return ParseResumeExpPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.Schools))
            {
                return ParseSchoolsPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.ImageWall))
            {
                return ParseImageWallPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.TextBlock))
            {
                return ParseTextBlockPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.GitHub))
            {
                return ParseGitHubPage(page);
            }
            else
            {
                return ParseEmptyPage(page);
            }
        }

        /// <summary>
        /// Parse a Page from the database into Business models
        /// </summary>
        /// <param name="inpages">The page to translate</param>
        /// <returns>Business model representing the page</returns>
        public static IEnumerable<BusinessModels.Page> ParsePages(
            DbModels.DatabaseContext dbContext, IEnumerable<DbModels.Page> inpages, ILogger log)
        {
            var pages = (from page in inpages
                         orderby page.Order
                         select page).ToList();
            return pages.Select(p => ParsePage(p)).ToList();
        }

        /// <summary>
        /// Get a specific page by its ID
        /// </summary>
        /// <param name="dbContext">DB Context to get page from</param>
        /// <param name="id">ID of the page to search for</param>
        /// <param name="log">Logging object to log information</param>
        /// <returns>Business object representing the page</returns>
        /// <exception cref="IndexOutOfRangeException">Invalid GUID string</exception>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public static async Task<BusinessModels.Page> GetById(
            DbModels.DatabaseContext dbContext, string id, ILogger log)
        {
            if (!Guid.TryParse(id, out Guid useGuid))
            {
                log.LogError("Invalid ID Passed, not appropriate Guid");
                throw new IndexOutOfRangeException();
            }

            var page = await dbContext.Pages.FirstOrDefaultAsync(p => useGuid.Equals(p.Id));

            if (page == null)
            {
                log.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return ParsePage(page);
        }
    }
}
