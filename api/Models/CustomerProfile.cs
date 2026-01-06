using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceApi.Models
{
    public class CustomerProfile
    {
        [Key]
        public int CustomerProfileId { get; set; }

        [Required]
        public string UserId { get; set; }

        
        public ApplicationUser User { get; set; }

        public int? AgentProfileId { get; set; } = 0;

        
        public AgentProfile? AgentProfile { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }

        [Required, MaxLength(50)]
        public string NomineeName { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

    }
}
