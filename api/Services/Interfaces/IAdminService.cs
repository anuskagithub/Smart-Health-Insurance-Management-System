using HealthInsuranceApi.DTOs.Admin;
using HealthInsuranceApi.DTOs.Agent;
using HealthInsuranceApi.DTOs.ClaimsOfficer;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.HospitalProvider;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminUserDto>> GetAllUsersAsync();
        Task CreateUserAsync(AdminUserCreateDto dto);
        Task UpdateUserAsync(string userId, AdminUserUpdateDto dto);
        Task DeleteUserAsync(string userId);
        Task ApproveUserAsync(string userId);
        Task AssignRolesAsync(string userId, List<string> roles);

        Task CreateCustomerProfileAsync(string userId, CustomerCreateDto dto);
        Task CreateAgentProfileAsync(string userId, AgentCreateDto dto);
        Task CreateClaimsOfficerProfileAsync(string userId, ClaimsOfficerCreateDto dto);
        Task CreateHospitalProfileAsync(string userId, HospitalProviderCreateDto dto);
    }
}
