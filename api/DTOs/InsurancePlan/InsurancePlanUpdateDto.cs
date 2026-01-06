namespace HealthInsuranceApi.DTOs.InsurancePlan
{
    public class InsurancePlanUpdateDto
    {
        public string PlanName { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public int DurationInMonths { get; set; }
        public string PlanType { get; set; }
    }
}
