using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthInsuranceApi.Models
{
    public class ClaimsOfficerProfile
    {
        [Key]
        public int ClaimsOfficerProfileId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required, MaxLength(50)]
        public string EmployeeCode { get; set; }

        [Required, MaxLength(100)]
        public string Department { get; set; }

        public int ApprovedClaimsCount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
