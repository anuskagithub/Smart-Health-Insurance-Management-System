using HealthInsuranceApi.Models;
using Microsoft.AspNetCore.Identity;

namespace HealthInsuranceApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // ---------------------------
            // ROLES
            // ---------------------------
            string[] roles =
            {
                "Admin",
                "Customer",
                "Agent",
                "ClaimsOfficer",
                "HospitalProvider"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // ---------------------------
            // ADMIN USER
            // ---------------------------
            var adminEmail = "admin@gmail.com";
            var adminUserName = "admin";
            var adminPassword = "Admin@123";

            var adminUser =
                await userManager.FindByNameAsync(adminUserName);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    IsApproved = true                     
                };

                var result =
                    await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
