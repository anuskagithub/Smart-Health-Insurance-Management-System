using HealthInsuranceApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<AgentProfile> AgentProfiles { get; set; }
        public DbSet<ClaimsOfficerProfile> ClaimsOfficerProfiles { get; set; }
        public DbSet<HospitalProvider> HospitalProviders { get; set; }

        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        // ========================
        // FLUENT API CONFIGURATION
        // ========================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ------------------------
            // ApplicationUser
            // ------------------------
            builder.Entity<ApplicationUser>()
                .Property(u => u.IsApproved)
                .HasDefaultValue(false);

           

            builder.Entity<CustomerProfile>()
                .Property(cp => cp.IsActive)
                .HasDefaultValue(false);

           

         
          
            // ------------------------
            // InsurancePlan
            // ------------------------
            builder.Entity<InsurancePlan>()
                .Property(ip => ip.IsActive)
                .HasDefaultValue(true);

        }
    }
}
