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
            var connectionChoice = Configuration.GetValue("UseConnection", "SqliteConn") ?? "";
            var connectionString = Configuration.GetConnectionString(connectionChoice);
            if (connectionChoice.StartsWith("Sqlite"))
                services.AddDbContext<DbModels.DatabaseContext>(opt =>
                    opt.UseSqlite(connectionString));
            else
                services.AddDbContext<DbModels.DatabaseContext>(opt =>
                    opt.UseLazyLoadingProxies().UseSqlServer(connectionString));

            var smtpSettings = Configuration.GetSection("SmtpSettings");
            services.Configure<BusinessModels.SmtpSettings>(smtpSettings);
            services.AddSingleton<Controllers.IMailer, Controllers.Mailer>();

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
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' subaruvagabond.com fonts.googleapis.com fonts.gstatic.com www.gstatic.com www.google.com docs.google.com");

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
