namespace HealthInsuranceApi.DTOs.Reports
{
    public class PoliciesByTypeStatusReportDto
    {
        
        public string PlanType { get; set; }

        // Active / Expired / Suspended
        public string PolicyStatus { get; set; }

        public int PolicyCount { get; set; }
    }
}
