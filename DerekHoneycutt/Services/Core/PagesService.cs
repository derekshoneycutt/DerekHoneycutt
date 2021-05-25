using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DerekHoneycutt.Services.Core
{
    /// <summary>
    /// Service for handling pages in the application
    /// </summary>
    public class PagesService : IPagesService
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
        /// Images services for handling images in the application
        /// </summary>
        private readonly IImagesService ImagesService;
        /// <summary>
        /// Images services for handling resume experience jobs in the application
        /// </summary>
        private readonly IResumeExpJobsService ResumeExpJobsService;
        /// <summary>
        /// Images services for handling schools in the application
        /// </summary>
        private readonly ISchoolsService SchoolsService;

        public PagesService(
            DbModels.DatabaseContext dbContext,
            ILogger<PagesService> logger,
            IImagesService imagesService,
            IResumeExpJobsService resumeExpJobService,
            ISchoolsService schoolsService)
        {
            DbContext = dbContext;
            Logger = logger;
            ImagesService = imagesService;
            ResumeExpJobsService = resumeExpJobService;
            SchoolsService = schoolsService;
        }

        /// <summary>
        /// Parse an empty page
        /// </summary>
        /// <param name="page">empty page to parse</param>
        /// <returns>A parsed, useless page</returns>
        private static BusinessModels.Page ParseEmptyPage(DbModels.Page page)
        {
            return new BusinessModels.Page()
            {
                PageOrigin = page,
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
        private async Task<BusinessModels.ImageWallPage> ParseImageWallPage(
            DbModels.Page page)
        {
            var retPage =  new BusinessModels.ImageWallPage()
            {
                PageOrigin = page,
                Id = page.Id,
                Type = BusinessModels.PageTypes.ImageWall,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                ImageWallPageOrigin = page.ImageWallExt,
                ImageWallId = page.ImageWallExt.Id,
                Description = page.ImageWallExt.Description
            };

            retPage.Images = await ImagesService.GetFromPage(retPage);

            return retPage;
        }

        /// <summary>
        /// Parse a Resume Experience page
        /// </summary>
        /// <param name="page">Resume Experience page to parse</param>
        /// <returns>A parsed Resume Experience page</returns>
        private async Task<BusinessModels.ResumeExpPage> ParseResumeExpPage(
            DbModels.Page page)
        {
            var retPage = new BusinessModels.ResumeExpPage()
            {
                PageOrigin = page,
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeExp,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                ResumeExpPageOrigin = page.ResumeExpExt,
                ResumeExpId = page.ResumeExpExt.Id
            };
            retPage.Jobs = await ResumeExpJobsService.GetFromPage(retPage);

            return retPage;
        }

        /// <summary>
        /// Parse a Resume Head page
        /// </summary>
        /// <param name="page">Resume Head page to parse</param>
        /// <returns>A parsed Resume Head page</returns>
        private static BusinessModels.ResumeHeadPage ParseResumeHeadPage(
            DbModels.Page page)
        {
            return new BusinessModels.ResumeHeadPage()
            {
                PageOrigin = page,
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeHead,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                ResumeHeadPageOrigin = page.ResumeHeadExt,
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
        private static BusinessModels.GitHubPage ParseGitHubPage(
            DbModels.Page page)
        {
            return new BusinessModels.GitHubPage()
            {
                PageOrigin = page,
                Id = page.Id,
                Type = BusinessModels.PageTypes.GitHub,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                GitHubPageOrigin = page.GitHubPageExt,
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
        private async Task<BusinessModels.SchoolsPage> ParseSchoolsPage(DbModels.Page page)
        {
            var retPage = new BusinessModels.SchoolsPage()
            {
                PageOrigin = page,
                Id = page.Id,
                Type = BusinessModels.PageTypes.ResumeExp,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = null,
                Orientation = page.Orientation,
                SchoolsPageOrigin = page.SchoolsExt,
                SchoolsId = page.SchoolsExt.Id
            };
            retPage.Schools = await SchoolsService.GetFromPage(retPage);

            return retPage;
        }

        /// <summary>
        /// Parse a Text Block page
        /// </summary>
        /// <param name="page">Text Block page to parse</param>
        /// <returns>A parsed Text Block page</returns>
        private static BusinessModels.TextBlockPage ParseTextBlockPage(
            DbModels.Page page)
        {
            return new BusinessModels.TextBlockPage()
            {
                PageOrigin = page,
                Id = page.Id,
                Type = BusinessModels.PageTypes.TextBlock,
                Title = page.Title,
                Subtitle = page.Subtitle,
                Background = page.Background,
                Image = page.Image,
                Orientation = page.Orientation,
                TextBlockPageOrigin = page.TextBlockExt,
                TextBlockId = page.TextBlockExt.Id,
                Text = page.TextBlockExt.Text
            };
        }

        /// <summary>
        /// Parse a page
        /// </summary>
        /// <param name="page">Page to parse</param>
        /// <returns>A parsed page</returns>
        private async Task<BusinessModels.Page> ParsePage(
            DbModels.Page page)
        {
            if (String.Equals(page.Type, BusinessModels.PageTypes.ResumeHead))
            {
                return ParseResumeHeadPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.ResumeExp))
            {
                return await ParseResumeExpPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.Schools))
            {
                return await ParseSchoolsPage(page);
            }
            else if (String.Equals(page.Type, BusinessModels.PageTypes.ImageWall))
            {
                return await ParseImageWallPage(page);
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
        /// Get the pages for a particular landing
        /// </summary>
        /// <param name="landing">Landing to get the pages for (can include just Id, but must included that)</param>
        /// <returns>Collection of pages for the specified landing</returns>
        public async Task<ICollection<BusinessModels.Page>> GetFromLanding(BusinessModels.Landing landing)
        {
            ICollection<DbModels.Page> pages;
            if (landing.LandingOrigin != null)
            {
                pages = (from page in landing.LandingOrigin.Pages
                         orderby page.Order
                         select page).ToList();
            }
            else
            {
                pages = await (from page in DbContext.Pages
                               where page.LandingId == landing.Id
                               orderby page.Order
                               select page).ToListAsync();
            }
            var ret = new List<BusinessModels.Page>(pages.Count);
            foreach (var page in pages)
            {
                ret.Add(await ParsePage(page));
            }
            return ret;
        }

        /// <summary>
        /// Get a specific page by its ID
        /// </summary>
        /// <param name="id">ID of the page to search for</param>
        /// <returns>Business object representing the page</returns>
        /// <exception cref="KeyNotFoundException">ID Passed was not discovered in database</exception>
        public async Task<BusinessModels.Page> GetById(Guid id)
        {
            var page = await DbContext.Pages.FirstOrDefaultAsync(p => id.Equals(p.Id));

            if (page == null)
            {
                Logger.LogError("Invalid ID Passed, not found");
                throw new KeyNotFoundException();
            }

            return await ParsePage(page);
        }
    }
}
