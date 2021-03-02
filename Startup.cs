using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DerekHoneycutt
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(conf =>
            {
                conf.AddDebug();
            });

            var connectionChoice = Configuration.GetValue("UseConnection", "Azure") ?? "";
            var connectionString = Configuration.GetConnectionString(connectionChoice);
            services.AddDbContext<DbModels.DatabaseContext>(opt =>
                    opt.UseLazyLoadingProxies().UseSqlServer(connectionString));

            //Add configuration patterns
            var smtpSettings = Configuration.GetSection("SmtpSettings");
            services.Configure<Config.SmtpSettings>(smtpSettings);

            //Add services for individual parts
            services.AddScoped<Services.IEmailService, Services.Core.EmailService>();
            services.AddScoped<Services.IImagesService, Services.Core.ImagesService>();
            services.AddScoped<Services.ILandingsService, Services.Core.LandingsService>();
            services.AddScoped<Services.IPagesService, Services.Core.PagesService>();
            services.AddScoped<Services.IResumeExpJobsService, Services.Core.ResumeExpJobsService>();
            services.AddScoped<Services.ISchoolsService, Services.Core.SchoolsService>();

            services.AddCors();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new RestModels.PageJsonConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' subaruvagabond.com fonts.googleapis.com fonts.gstatic.com www.gstatic.com www.google.com docs.google.com dc.services.visualstudio.com");

                await next();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    var headers = context.Context.Response.Headers;

                    headers.Add("Cache-Control", "no-cache");
                }
            });

            app.UseMiddleware<Middlewares.ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
