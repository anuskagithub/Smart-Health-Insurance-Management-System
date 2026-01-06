namespace HealthInsuranceApi.DTOs.Agent
{
    public class AgentReadDto
    {
        public int AgentProfileId { get; set; }
        public string AgentCode { get; set; }
        public int YearsOfExperience { get; set; }
        public string Region { get; set; }
        public bool IsActive { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }
    }
}
