using HealthInsuranceApi.DTOs.Policy;


namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IPolicyService
    {
        Task<int> EnrollPolicyAsync(PolicyCreateDto dto);
        Task RenewPolicyAsync(int policyId);
        Task SuspendPolicyAsync(int policyId);
        Task<IEnumerable<PolicyReadDto>> GetPoliciesAsync();

        Task<PolicyReadDto?> GetPolicyByIdAsync(int policyId);

        Task<IEnumerable<PolicyTypeStatusReportDto>> GetPolicyTypeStatusReportAsync();
    }
}

