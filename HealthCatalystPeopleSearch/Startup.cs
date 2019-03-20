using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO.Compression;
using VanderStack.HealthCatalystPeopleSearch.PersonFeature;

namespace VanderStack.HealthCatalystPeopleSearch
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
			services
				.AddOptions()
				.Configure<AppSettings>(Configuration)
				.AddDbContext<PersonDatabaseContext>(options => {
					var appSettings = new AppSettings();
					Configuration.Bind(appSettings);

					if (appSettings.UseSqlDatabase)
					{
						options.UseSqlServer(appSettings.SqlDatabaseConnectionString);
					}
					else if (appSettings.UseInMemoryDatabase)
					{
						options.UseInMemoryDatabase(appSettings.InMemoryDatabaseName);
					}
				})
				.Configure<GzipCompressionProviderOptions>(options =>
					options.Level = CompressionLevel.Optimal
				)
				.AddResponseCompression()
				.AddMvc()
			;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseResponseCompression();

            app.UseStaticFiles();

			// When configured initialize the database for first time use
			var appSettings = app.ApplicationServices.GetRequiredService<IOptions<AppSettings>>().Value;
			if (appSettings.UseSqlDatabase && appSettings.SeedSqlDatabaseOnStart)
			{
				try
				{
					var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
					using (var serviceScope = serviceScopeFactory.CreateScope())
					{
						serviceScope
							.ServiceProvider
							.GetService<PersonDatabaseContext>()
							.Database
							.Migrate()
						;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
				}
			}
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
