namespace HealthInsuranceApi.DTOs.ClaimsOfficer
{
    public class ClaimsOfficerCreateDto
    {
        public string EmployeeCode { get; set; }
        public string Department { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }
    }
}
