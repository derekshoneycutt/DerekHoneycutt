using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace DerekHoneycutt.Controllers
{
    /// <summary>
    /// Defines the primary controller for the portfolio methods of the application
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PortfolioController : Controller
    {
        /// <summary>
        /// Database Context that the application will run on
        /// </summary>
        private readonly DbModels.DatabaseContext _dbContext;
        /// <summary>
        /// Mailer to use if the user is trying to contact me
        /// </summary>
        private readonly IMailer _mailer;
        /// <summary>
        /// Logger to log any information as we progress
        /// </summary>
        private readonly ILogger<PortfolioController> _logger;
        /// <summary>
        /// Configuration object to retrieve
        /// </summary>
        private readonly IConfiguration Configuration;

        public PortfolioController(
            DbModels.DatabaseContext dbContext,
            IMailer mailer,
            ILogger<PortfolioController> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mailer = mailer;
            _logger = logger;
            Configuration = configuration;
        }

        /// <summary>
        /// Translate a ResumeExpJob from the business model to a REST return model
        /// </summary>
        /// <param name="rej">Business model to translate</param>
        /// <returns>REST object to return to the client</returns>
        private RestModels.ResumeExpJob TranslateResumeExpJob(BusinessModels.ResumeExpJob rej)
        {
            return new RestModels.ResumeExpJob()
            {
                Self = rej.Id.ToString(),
                Title = rej.Title,
                Employer = rej.Employer,
                EmployerCity = rej.EmployerCity,
                StartDate = rej.StartDate,
                EndDate = rej.EndDate,
                Description = rej.Description,
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/resumeexpjob/{rej.Id.ToString()}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// Translate a School business model to the REST model for the client
        /// </summary>
        /// <param name="school">Business model to translate</param>
        /// <returns>REST model to return to client</returns>
        private RestModels.School TranslateSchool(BusinessModels.School school)
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
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/school/{school.Id.ToString()}",
                        Method= "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// Translate a Page business model into a REST model
        /// </summary>
        /// <param name="page">Business model to translate</param>
        /// <returns>REST model to return to the client</returns>
        private RestModels.Page TranslatePage(BusinessModels.Page page)
        {
            RestModels.Page ret = null;

            if (page is BusinessModels.ImageWallPage iwpage)
            {
                ret = new RestModels.ImageWallPage()
                {
                    Description = iwpage.Description,
                    Images = iwpage.Images.Split('|'),
                    Type = BusinessModels.PageTypes.ImageWall
                };
            }
            else if (page is BusinessModels.ResumeExpPage repage)
            {
                ret = new RestModels.ResumeExpPage()
                {
                    Type = BusinessModels.PageTypes.ResumeExp,
                    Jobs = repage.Jobs.Select(rej => TranslateResumeExpJob(rej)).ToList()
                };
            }
            else if (page is BusinessModels.ResumeHeadPage rhpage)
            {
                ret = new RestModels.ResumeHeadPage()
                {
                    Type = BusinessModels.PageTypes.ResumeHead,
                    Description = rhpage.Description,
                    Competencies = rhpage.Competencies
                };
            }
            else if (page is BusinessModels.SchoolsPage spage)
            {
                ret = new RestModels.SchoolsPage()
                {
                    Type = BusinessModels.PageTypes.Schools,
                    Schools = spage.Schools.Select(school => TranslateSchool(school)).ToList()
                };
            }
            else if (page is BusinessModels.TextBlockPage tbpage)
            {
                ret = new RestModels.TextBlockPage()
                {
                    Text = tbpage.Text
                };
            }

            if (ret != null)
            {
                ret.Self = page.Id.ToString();
                ret.Title = page.Title;
                ret.Subtitle = page.Subtitle;
                ret.Image = page.Image;
                ret.Background = page.Background;
                ret.Orientation = page.Orientation;
                ret.Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/page/{page.Id.ToString()}",
                        Method= "GET",
                        PostData = null
                    }
                };
            }

            return ret;
        }

        /// <summary>
        /// Translate a Landing business model into a REST model
        /// </summary>
        /// <param name="landing">Business model to translate</param>
        /// <returns>REST model to return to the client</returns>
        private RestModels.Landing TranslateLanding(BusinessModels.Landing landing)
        {
            return new RestModels.Landing()
            {
                Self = landing.Id.ToString(),
                Href = landing.Href,
                Title = landing.Title,
                Subtitle = landing.Subtitle,
                Icon = landing.Icon,
                Order = landing.Order,
                Pages = landing.Pages.Select(p => TranslatePage(p)).ToList(),
                Links = new[]
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = $"portfolio/landing/{landing.Id.ToString()}",
                        Method = "GET",
                        PostData = null
                    }
                }
            };
        }

        /// <summary>
        /// REST point for getting a particular landing
        /// </summary>
        /// <param name="landingid">ID of the landing to retrieve</param>
        /// <returns>Action result, hopefully including landing</returns>
        [HttpGet("landing/{landingid}")]
        public async Task<IActionResult> GetLanding(string landingid)
        {
            BusinessModels.Landing landing;

            try
            {
                landing = await DbBusiness.Landings.GetById(_dbContext, landingid, _logger);
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

            return new OkObjectResult(TranslateLanding(landing));
        }

        /// <summary>
        /// REST point for getting a particular page
        /// </summary>
        /// <param name="pageid">ID of the page to retrieve</param>
        /// <returns>Action result, hopefully including page</returns>
        [HttpGet("page/{pageid}")]
        public async Task<IActionResult> GetPage(string pageid)
        {
            BusinessModels.Page page;

            try
            {
                page = await DbBusiness.Pages.GetById(_dbContext, pageid, _logger);
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

            return new OkObjectResult(TranslatePage(page));
        }

        /// <summary>
        /// REST point for getting a particular ResumeExp Job
        /// </summary>
        /// <param name="jobid">ID of the job to retrieve</param>
        /// <returns>Action result, hopefully including job</returns>
        [HttpGet("resumeexpjob/{jobid}")]
        public async Task<IActionResult> GetResumeExpJob(string jobid)
        {
            BusinessModels.ResumeExpJob job;

            try
            {
                job = await DbBusiness.ResumeExpJobs.GetById(_dbContext, jobid, _logger);
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

            return new OkObjectResult(TranslateResumeExpJob(job));
        }

        /// <summary>
        /// REST point for getting a particular school
        /// </summary>
        /// <param name="landingid">ID of the school to retrieve</param>
        /// <returns>Action result, hopefully including school</returns>
        [HttpGet("school/{schoolid}")]
        public async Task<IActionResult> GetSchool(string schoolid)
        {
            BusinessModels.School school;

            try
            {
                school = await DbBusiness.Schools.GetById(_dbContext, schoolid, _logger);
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

        /// <summary>
        /// Initial entry point for the portfolio application
        /// </summary>
        /// <returns>Action result., hopefully containing all needed info for application to continue</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var landings = await DbBusiness.Landings.GetAll(_dbContext, _logger);

            return new OkObjectResult(new RestModels.Home()
            {
                Landings = landings.Select(landing => TranslateLanding(landing)).ToList(),
                Links = new []
                {
                    new RestModels.Link()
                    {
                        Rel = "self",
                        Href = "portfolio",
                        Method = "GET",
                        PostData = null
                    }/*,  // Keep this, just in case? IDK.
                    new RestModels.Link()
                    {
                        Rel = "Contact",
                        Href = "portfolio/contact",
                        Method = "POST",
                        PostData = new RestModels.PostContact()
                        {
                            From = "Name",
                            Return = "person@example.com",
                            Message = "Any plain text to send"
                        }
                    }*/
                }
            });
        }

        /// <summary>
        /// POST access point for contacting the webmaster
        /// </summary>
        /// <param name="form">Contact information, including who is sending the message and the message</param>
        /// <returns>Action result describing success or failure object</returns>
        [HttpPost("contact")]
        public async Task<IActionResult> PostContact(RestModels.PostContact form)
        {
            return new OkObjectResult(new { Message = "Message not actually sent." });

            /*var sendto = Configuration.GetValue("ContactEmail", "derekhoneycutthole@mailinator.com");

            await _mailer.SendEmailAsync(sendto, form);

            return new OkObjectResult(new { Message = "Sent Successful!" });*/
        }
    }
}