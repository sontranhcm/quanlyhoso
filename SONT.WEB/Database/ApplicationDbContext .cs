using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SONT.WEB.Entities;
using System.Reflection.Emit;

namespace SONT.WEB.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
               UserName = "admin",
               Email = "son.tranminhhcm@gmail.com",
               EmailConfirmed = true,
               PhoneNumberConfirmed = true,
               SecurityStamp = Guid.NewGuid().ToString("D"),
            }
        );
        }
    }
}
