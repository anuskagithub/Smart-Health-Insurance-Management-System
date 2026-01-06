using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.InsurancePlan;
using HealthInsuranceApi.DTOs.Payment;
using HealthInsuranceApi.DTOs.Policy;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface ICustomerService
    {
        // View my policies
        Task<IEnumerable<PolicyReadDto>> GetMyPoliciesAsync(string customerUserId);

        // Pay premium
        Task PayPremiumAsync(PaymentCreateDto dto, string customerUserId);

        // Submit claim WITH NOTES
        Task SubmitClaimAsync(
            ClaimCreateDto dto,
            string customerUserId);

        // Track claims
        Task<IEnumerable<ClaimReadDto>> GetMyClaimsAsync(string customerUserId);

        Task<ClaimReadDto?> GetMyClaimByIdAsync(
            int claimId,
            string customerUserId);

        Task<IEnumerable<InsurancePlanReadDto>> GetAvailablePlansAsync();

        Task CreateCustomerProfileAsync(CustomerCreateDto dto, string userId);

        Task<CustomerReadDto> GetCustomerProfileId(string userId);


    }
}
