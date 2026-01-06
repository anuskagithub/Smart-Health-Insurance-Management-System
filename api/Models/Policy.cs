using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceApi.Models
{
    public class Policy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required, MaxLength(50)]
        public string PolicyNumber { get; set; }

        [Required]
        public int InsurancePlanId { get; set; }

        
        public InsurancePlan InsurancePlan { get; set; }

        [Required]
        public int CustomerProfileId { get; set; }

        
        public CustomerProfile CustomerProfile { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }
        // Active / Expired / Suspended

        public decimal PremiumAmount { get; set; }
        public decimal CoverageRemaining { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
