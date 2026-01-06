namespace HealthInsuranceApi.DTOs.Reports
{
    public class PremiumVsPayoutReportDto
    {
        public decimal TotalPremiumCollected { get; set; }
        public decimal TotalClaimPayout { get; set; }
    }
}
