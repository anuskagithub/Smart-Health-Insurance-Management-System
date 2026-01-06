using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceApi.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        // Premium OR ClaimPayout
        [Required]
        public string PaymentType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        // Pending / Completed
        [Required]
        public string Status { get; set; } = "Pending";

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        // -------------------------
        // RELATIONS
        // -------------------------
        public int? PolicyId { get; set; }
        
        public Policy Policy { get; set; }

        public int? ClaimId { get; set; }
        
        public Claim Claim { get; set; }
    }
}
