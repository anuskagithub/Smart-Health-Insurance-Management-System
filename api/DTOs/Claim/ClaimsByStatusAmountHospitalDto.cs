namespace HealthInsuranceApi.DTOs.Claim
{
    public class ClaimsByStatusAmountHospitalDto
    {
        public string ProviderName { get; set; }
        public string Status { get; set; }
        public int ClaimCount { get; set; }
        public decimal TotalClaimAmount { get; set; }
    }
}
