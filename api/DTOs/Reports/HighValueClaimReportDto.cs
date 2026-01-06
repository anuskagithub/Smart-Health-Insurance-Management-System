namespace HealthInsuranceApi.DTOs.Reports
{
    public class HighValueClaimReportDto
    {
        public int ClaimId { get; set; }
        public string PolicyNumber { get; set; }
        public string ProviderName { get; set; }
        public decimal ClaimAmount { get; set; }

        public string Status { get; set; }
    }
}
