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
            DbModels.Page page, DbModels.ImageWallPage imageWallExt)
        {
            if (imageWallExt == null)
                throw new ArgumentException("Page must be Image Wall to parse as such");

            return new BusinessModels.ImageWallPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ImageWall,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                ImageWallId = imageWallExt.Id,
                Description = imageWallExt.Description,
                Images = imageWallExt.Images
            };
        }

        /// <summary>
        /// Parse a Resume Experience page
        /// </summary>
        /// <param name="page">Resume Experience page to parse</param>
        /// <returns>A parsed Resume Experience page</returns>
        public static BusinessModels.ResumeExpPage ParseResumeExpPage(
            DbModels.DatabaseContext dbContext,
            DbModels.Page page, DbModels.ResumeExpPage resumeExpExt,
            ILogger log)
        {
            if (resumeExpExt == null)
                throw new ArgumentException("Page must be Resume Experience to parse as such");

            return new BusinessModels.ResumeExpPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeExp,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                ResumeExpId = resumeExpExt.Id,
                Jobs = resumeExpExt.Jobs != null ?
                            resumeExpExt.Jobs.Select(j => ResumeExpJobs.ParseJob(j)).ToList() :
                            (from job in dbContext.ResumeExpJobs
                             where job.PageId == resumeExpExt.Id
                             select ResumeExpJobs.ParseJob(job)).ToList()
            };
        }

        /// <summary>
        /// Parse a Resume Head page
        /// </summary>
        /// <param name="page">Resume Head page to parse</param>
        /// <returns>A parsed Resume Head page</returns>
        public static BusinessModels.ResumeHeadPage ParseResumeHeadPage(
            DbModels.Page page, DbModels.ResumeHeadPage resumeHeadExt)
        {
            if (resumeHeadExt == null)
                throw new ArgumentException("Page must be Resume Head to parse as such");

            return new BusinessModels.ResumeHeadPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeHead,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                ResumeHeadId = resumeHeadExt.Id,
                Description = resumeHeadExt.Description,
                Competencies = resumeHeadExt.Competencies
            };
        }

        /// <summary>
        /// Parse a Resume GitHub page
        /// </summary>
        /// <param name="page">Resume GitHub page to parse</param>
        /// <returns>A parsed Resume GitHub page</returns>
        public static BusinessModels.GitHubPage ParseGitHubPage(
            DbModels.Page page, DbModels.GitHubPage resumeHeadExt)
        {
            if (resumeHeadExt == null)
                throw new ArgumentException("Page must be GitHub Page to parse as such");

            return new BusinessModels.GitHubPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeHead,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                GitHubId = resumeHeadExt.Id,
                GitHub = resumeHeadExt.GitHub,
                Description = resumeHeadExt.Description
            };
        }

        /// <summary>
        /// Parse a Schools page
        /// </summary>
        /// <param name="page">Schools page to parse</param>
        /// <returns>A parsed Schools page</returns>
        public static BusinessModels.SchoolsPage ParseSchoolsPage(
            DbModels.DatabaseContext dbContext,
            DbModels.Page page, DbModels.SchoolsPage schoolsExt,
            ILogger log)
        {
            if (schoolsExt == null)
                throw new ArgumentException("Page must be Schools to parse as such");

            return new BusinessModels.SchoolsPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.Schools,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                SchoolsId = schoolsExt.Id,
                Schools = (schoolsExt.Schools != null) ?
                            schoolsExt.Schools.Select(j => Schools.Parse(j)).ToList() :
                            (from school in dbContext.Schools
                             where school.PageId == schoolsExt.Id
                             select Schools.Parse(school)).ToList()
            };
        }

        /// <summary>
        /// Parse a Text Block page
        /// </summary>
        /// <param name="page">Text Block page to parse</param>
        /// <returns>A parsed Text Block page</returns>
        public static BusinessModels.TextBlockPage ParseTextBlockPage(
            DbModels.Page page, DbModels.TextBlockPage textBlockExt)
        {
            if (page.TextBlockExt == null)
                throw new ArgumentException("Page must be TextBlock to parse as such");

            return new BusinessModels.TextBlockPage()
            {
                Id = page.Id,
                Type = BusinessModels.PageTypes.TextBlock,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                TextBlockId = textBlockExt.Id,
                Text = textBlockExt.Text
            };
        }

        /// <summary>
        /// Parse a page
        /// </summary>
        /// <param name="page">Page to parse</param>
        /// <returns>A parsed page</returns>
        public static BusinessModels.Page ParsePage(
            DbModels.DatabaseContext dbContext, DbModels.Page page, ILogger log)
        {
            if (page.ImageWallExt != null)
                return ParseImageWallPage(page, page.ImageWallExt);
            if (page.ResumeExpExt != null)
                return ParseResumeExpPage(dbContext, page, page.ResumeExpExt, log);
            if (page.ResumeHeadExt != null)
                return ParseResumeHeadPage(page, page.ResumeHeadExt);
            if (page.GitHubPageExt != null)
                return ParseGitHubPage(page, page.GitHubPageExt);
            if (page.SchoolsExt != null)
                return ParseSchoolsPage(dbContext, page, page.SchoolsExt, log);
            if (page.TextBlockExt != null)
                return ParseTextBlockPage(page, page.TextBlockExt);

            return ParseEmptyPage(page);
        }

        /// <summary>
        /// Parse a Page from the database into Business models
        /// </summary>
        /// <param name="inpages">The page to translate</param>
        /// <returns>Business model representing the page</returns>
        public static IEnumerable<BusinessModels.Page> ParsePages(
            DbModels.DatabaseContext dbContext, IEnumerable<DbModels.Page> inpages, ILogger log)
        {
            return from page in inpages
                   orderby page.Order
                   select ParsePage(dbContext, page, log);
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

            return ParsePage(dbContext, page, log);
        }
    }
}
