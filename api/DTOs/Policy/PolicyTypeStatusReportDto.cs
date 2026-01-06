namespace HealthInsuranceApi.DTOs.Policy
{
    public class PolicyTypeStatusReportDto
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; }
        public string PlanName { get; set; }

        public List<PolicyStatusCount> PolicyStatusCount { get; set; }
    }
}
