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
            Action<DbContextOptionsBuilder> optsBuilder = null;
            if (connectionChoice.StartsWith("Sqlite"))
                optsBuilder = opt =>
                    opt.UseSqlite(Configuration.GetConnectionString(connectionChoice));
            else
                optsBuilder = opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString(connectionChoice));
            services.AddDbContext<DbModels.DatabaseContext>(optsBuilder);

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
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' subaruvagabond.com fonts.googleapis.com fonts.gstatic.com");

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

                    headers.Add("Cache-Control", "max-age=0");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
