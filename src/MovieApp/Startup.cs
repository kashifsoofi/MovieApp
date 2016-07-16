using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MovieApp
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Register Entity Framework
            services.AddEntityFramework()
                .AddEntityFrameworkSqlServer()
                .AddDbContext<MovieAppContext>(options =>options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            // Add ASP.NET Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MovieAppContext>();

            services.Configure<AuthorizationOptions>(options =>
                options.AddPolicy("CanEdit", policy => policy.RequireClaim("CanEdit", "true"))
            );

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseIISPlatformHandler();

            app.UseIdentity();

            // Add MVC to the request pipeline.
            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });
            CreateSampleData(app.ApplicationServices).Wait();
        }

        private static async Task CreateSampleData(IServiceProvider applicationServices)
        {
            using (var dbContext = applicationServices.GetService<MovieAppContext>())
            {
                var sqlServerDatabase = dbContext.Database;
                if (sqlServerDatabase != null)
                {
                    // Create database in user root (c:\users\your name)
                    if (await sqlServerDatabase.EnsureCreatedAsync())
                    {
                        // add some movies
                        var movies = new List<Movie>
                        {
                            new Movie {Title="Star Wars", Director="Lucas"},
                            new Movie {Title="King Kong", Director="Jackson"},
                            new Movie {Title="Memento", Director="Nolan"}
                        };
                        movies.ForEach(m => dbContext.Movies.Add(m));

                        // add some users
                        var userManager = applicationServices.GetService<UserManager<ApplicationUser>>();

                        // add editor user
                        var stephen = new ApplicationUser
                        {
                            UserName = "Admin"
                        };
                        var result = await userManager.CreateAsync(stephen, "P@ssw0rd");
                        await userManager.AddClaimAsync(stephen, new Claim("CanEdit", "true"));

                        // add normal user
                        var bob = new ApplicationUser
                        {
                            UserName = "Viewer"
                        };
                        await userManager.CreateAsync(bob, "P@ssw0rd");
                    }

                }
            }
        }

        // Entry point for the application.
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
