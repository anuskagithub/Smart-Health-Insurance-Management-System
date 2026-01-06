using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceApi.Models
{
    public class InsurancePlan
    {
        [Key]
        public int InsurancePlanId { get; set; }

        [Required, MaxLength(100)]
        public string PlanName { get; set; }
        
        [Required]
        public string PlanType { get; set; }

        [Required]
        public decimal PremiumAmount { get; set; }

        [Required]
        public decimal CoverageAmount { get; set; }

        [Required]
        public int DurationInMonths { get; set; } = 12;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
