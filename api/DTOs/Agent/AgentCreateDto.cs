using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceApi.DTOs.Agent
{
    public class AgentCreateDto
    {
        public string? UserId { get; set; }   // set in controller

        [Required, MaxLength(20)]
        public string AgentCode { get; set; }

        public int YearsOfExperience { get; set; }

        public string Region { get; set; }
    }
}
