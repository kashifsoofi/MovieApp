using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MovieApp.Models
{
    public class ApplicationUser : IdentityUser { }

    public class MoviesAppContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }

        public MoviesAppContext(DbContextOptions options)
            : base(options)
        { }

        public async Task EnsureSeedData(UserManager<ApplicationUser> userManager)
        {
            if (!this.Users.Any(u => u.UserName == "admin"))
            {
                // add some users
                var adminUser = new ApplicationUser
                {
                    UserName = "admin"
                };
                var result = await userManager.CreateAsync(adminUser, "P@ssw0rd");
                await userManager.AddClaimAsync(adminUser, new Claim("CanEdit", "true"));

                var bobUser = new ApplicationUser
                {
                    UserName = "bob"
                };
                await userManager.CreateAsync(bobUser, "P@ssw0rd");

                // add some movies
                var movies = new List<Movie>
                {
                    new Movie {Title="Star Wars", Director="Lucas"},
                    new Movie {Title="King Kong", Director="Jackson"},
                    new Movie {Title="Memento", Director="Nolan"}
                };
                movies.ForEach(m => Movies.Add(m));
                await SaveChangesAsync();
            }
        }
    }
}
