namespace HealthInsuranceApi.DTOs.ClaimsOfficer
{
    public class ClaimsOfficerReadDto
    {
        public int ClaimsOfficerProfileId { get; set; }

        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public int ApprovedClaimsCount { get; set; }
        public bool IsActive { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }
    }
}
