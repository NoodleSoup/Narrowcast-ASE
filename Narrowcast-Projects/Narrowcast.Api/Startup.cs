using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Narrowcast.Api.Domain;
using Narrowcast.Api.Logging;
using Narrowcast.Api.Repositories;
using Narrowcast.Api.Settings;
using System.Data;

namespace Narrowcast.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add all services to the application this contains the Database connection string,
        /// HealthChecks and adds the Interfaces as well as the controllers.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Get<AppSettings>();

            services.AddTransient<INarrowcastReadRepository, NarrowcastReadRepository>();
            services.AddTransient<IDbConnection>(db => new MySqlConnection(AppSettings.Database.Connection));

            services.AddSingleton<ILog, NLogger>();

            services.AddMvcCore();
            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                // Use the default property (Pascal) casing.
                options.JsonSerializerOptions.PropertyNamingPolicy = default;
            });

            services.AddHttpContextAccessor();

            services.AddCors(options =>
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                }));

            // Add healthcheck to service configuration
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
            hcBuilder.AddMySql(AppSettings.Database.Connection, name: "Narrowcasting-check", tags: new string[] { "narrowcast" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Enables the added services in the application, sets the endpoints for the HealthChecks,
        /// adds a global error handler, and handles CORS settings.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILog logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Add healthcheck to the application
            app.UseHealthChecks("/narrowcast/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/narrowcast/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.ConfigureExceptionHandler(logger);

            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}