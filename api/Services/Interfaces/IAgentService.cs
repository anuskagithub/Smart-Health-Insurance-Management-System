using HealthInsuranceApi.DTOs.Agent;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.Policy;


namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IAgentService
    {
        // ✅ NEW: Agent enrolls customer
        Task EnrollCustomerAsync(string agentUserId, CustomerCreateDto dto);

        // Existing
        Task CreatePolicyAsync(PolicyCreateDto dto);
        Task<IEnumerable<PolicyReadDto>> GetPoliciesByAgentAsync(string agentUserId);

        Task CreateAsync(AgentCreateDto dto, string userId);
        Task UpdateAsync(int ProfileId, AgentUpdateDto dto);

        Task<IEnumerable<AgentReadDto>> GetAllAgentAsync();
    }
}
