using Microsoft.AspNetCore.Identity;

namespace HealthInsuranceApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsApproved { get; set; } = false; // Admin approval required
        public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;

        public string? FullName { get; set; }

        public string? Address { get; set; }

    }
}
