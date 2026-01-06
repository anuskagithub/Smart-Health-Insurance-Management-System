namespace HealthInsuranceApi.DTOs.Reports
{
    public class PlanWisePolicyClaimCountDto
    {
        public string PlanName { get; set; }
        public int PolicyCount { get; set; }
        public int ClaimCount { get; set; }
    }
}
