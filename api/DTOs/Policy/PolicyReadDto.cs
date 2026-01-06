namespace HealthInsuranceApi.DTOs.Policy
{
    public class PolicyReadDto
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string PlanName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageRemaining { get; set; }
    }
}
