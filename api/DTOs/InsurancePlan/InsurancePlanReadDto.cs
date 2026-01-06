namespace HealthInsuranceApi.DTOs.InsurancePlan
{
    public class InsurancePlanReadDto
    {
        public int InsurancePlanId { get; set; }
        public string PlanName { get; set; }

        public string PlanType { get; set; }

        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public int DurationInMonths { get; set; }
        public bool IsActive { get; set; }
    }
}
