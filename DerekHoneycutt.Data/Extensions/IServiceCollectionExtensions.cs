using DerekHoneycutt.Data.Options;
using DerekHoneycutt.Data.Services.Implementation;
using DerekHoneycutt.Data.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DerekHoneycutt.Data.Extensions
{
    /// <summary>
    /// Extensions to the IServiceCollection class
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add the DerekHoneycutt Data Services to the collection
        /// </summary>
        /// <param name="services">Services collection to add to</param>
        /// <param name="Configuration">Configuration object to utilize in setup</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddDerekHoneycuttServices(
            this IServiceCollection services, IConfiguration Configuration)
        {

            var connectionChoice = Configuration.GetValue("UseConnection", "Azure") ?? "";
            var connectionString = Configuration.GetConnectionString(connectionChoice);
            services.AddDbContext<DbModels.DatabaseContext>(opt =>
                    opt.UseLazyLoadingProxies().UseSqlServer(connectionString));

            //Add configuration patterns
            services.AddOptions<SmtpSettings>()
                .Bind(Configuration.GetSection(nameof(SmtpSettings)));

            //Add services for individual parts
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<ILandingsService, LandingsService>();
            services.AddScoped<IPagesService, PagesService>();
            services.AddScoped<IResumeExpJobsService, ResumeExpJobsService>();
            services.AddScoped<ISchoolsService, SchoolsService>();

            return services;
        }
    }
}
