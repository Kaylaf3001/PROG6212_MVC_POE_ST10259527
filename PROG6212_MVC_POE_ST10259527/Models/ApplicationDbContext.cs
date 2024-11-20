using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfileModel> UserProfiles { get; set; }
        public DbSet<ClaimsModel> Claims { get; set; }
    }
}
