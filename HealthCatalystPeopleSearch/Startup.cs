using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			var databaseId = "VanderStackHealthCatalyst";
			var useInMemoryDb = true;
			var useSqlServerDb = !useInMemoryDb;

			services
				.AddOptions()
				.Configure<PersonSearchOptions>(options =>
					// todo: this can also come from environment variables
					// or a settings file depending on our deployment model
					options.MaxNumberOfResults = 100
				)
				.AddDbContext<PersonDatabaseContext>(options => {
					if (useSqlServerDb)
					{
						// Heads Up! the connection string should not be committed to source control.
						// This is done for simplicity of code sharing only.
						// In production this should come from an environment variable or a secrets vault.
						var connectionString = $@"Server=(localdb)\mssqllocaldb;Database={databaseId};Trusted_Connection=True;ConnectRetryCount=0";
						options.UseSqlServer(connectionString);
					}
					else if (useInMemoryDb)
					{
						options.UseInMemoryDatabase(databaseId);
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

			// Initialize the database for first time use
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
