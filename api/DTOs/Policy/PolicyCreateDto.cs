namespace HealthInsuranceApi.DTOs.Policy
{
    public class PolicyCreateDto
    {
        public int InsurancePlanId { get; set; }
        public int CustomerProfileId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
