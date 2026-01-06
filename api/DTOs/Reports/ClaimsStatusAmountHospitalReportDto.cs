namespace HealthInsuranceApi.DTOs.Reports
{
    public class ClaimsStatusAmountHospitalReportDto
    {
        // Submitted / Approved / Rejected / Paid
        public string ClaimStatus { get; set; }

        // Hospital / Clinic / Lab name
        public string ProviderName { get; set; }

        public int ClaimCount { get; set; }

        public decimal TotalClaimAmount { get; set; }
    }
}
