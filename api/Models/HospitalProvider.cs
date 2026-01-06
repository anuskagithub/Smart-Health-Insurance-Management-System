using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceApi.Models
{
    public class HospitalProvider
    {
        [Key]
        public int HospitalProviderId { get; set; }

        // Link to Identity user (login account)
        [Required]
        public string UserId { get; set; }

     
        public ApplicationUser User { get; set; }

        // Display / business name
        [Required, MaxLength(150)]
        public string ProviderName { get; set; }

        // Hospital, Clinic, Lab, NursingHome, DiagnosticCenter, etc.
        [Required, MaxLength(50)]
        public string ProviderType { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        // Whether this provider is part of the insurer’s network
        public bool IsNetworkProvider { get; set; } = true;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
