namespace HealthInsuranceApi.DTOs.Reports
{
    public class HospitalWiseClaimStatsDto
    {
        public int HospitalProviderId { get; set; }

        public string HospitalName { get; set; }

        // Total number of claims submitted
        public int TotalClaims { get; set; }

        // Sum of all claim amounts
        public decimal TotalClaimAmount { get; set; }

        // Sum of approved claim amounts
        public decimal ApprovedClaimAmount { get; set; }
    }
}
