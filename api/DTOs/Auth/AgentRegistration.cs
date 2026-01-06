using HealthInsuranceApi.DTOs.Agent;

namespace HealthInsuranceApi.DTOs.Auth
{
    public class AgentRegistration
    {
        public RegisterDto RegisterInfo { get; set; }
        public AgentCreateDto AgentProfileInfo { get; set; }
    }
}
