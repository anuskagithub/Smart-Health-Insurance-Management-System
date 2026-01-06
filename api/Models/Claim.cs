using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceApi.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }

        
        public int? PolicyId { get; set; }

       
        public Policy? Policy { get; set; }


        public int? HospitalProviderId { get; set; }

      
        public HospitalProvider? HospitalProvider { get; set; }

        [Required]
        public decimal ClaimAmount { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }
        // Submitted → InReview → Approved → Paid / Rejected       

        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;

        public string? Remarks { get; set; }
        public string? TreatmentHistory { get; set; }
        public string? ClaimOfficerComment { get; set; }
    }
}
