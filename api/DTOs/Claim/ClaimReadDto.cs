namespace HealthInsuranceApi.DTOs.Claim
{
    public class ClaimReadDto
    {
        public int ClaimId { get; set; }

        public int? PolicyId { get; set; }

        public string PolicyNumber { get; set; }
        public string ProviderName { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedOn { get; set; }

        public string Remarks { get; set; }
        public string TreatmentHistory { get; set; }
        public string ClaimOfficerComment { get; set; }
    }
}
