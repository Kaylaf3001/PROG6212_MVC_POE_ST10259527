using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PROG6212_MVC_POE_ST10259527.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<UserProfileModel> UserProfile { get; set; }
        public DbSet<ClaimsModel> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfileModel>()
                .HasKey(u => u.userID);

            modelBuilder.Entity<ClaimsModel>()
                .HasKey(c => c.ClaimID);
        }
    }
}
