using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.HospitalProvider;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IHospitalProviderService
    {
        // =====================================================
        // HOSPITAL / PROVIDER ACTIONS (ROLE: HospitalProvider)
        // =====================================================

        // Submit treatment details (claim) WITH NOTES
        Task SubmitClaimAsync(
            ClaimCreateDto dto,
            string hospitalProviderUserId);

        // View claims submitted by this provider
        Task<IEnumerable<ClaimReadDto>>
            GetClaimsByProviderAsync(string hospitalProviderUserId);

        // =====================================================
        // ADMIN MANAGEMENT (ROLE: Admin)
        // =====================================================

        // Create hospital / provider
        Task CreateAsync(HospitalProviderCreateDto dto, string userId);

        // Update hospital / provider details
        Task UpdateAsync(int providerId, HospitalProviderUpdateDto dto);

        // View all hospitals / providers
        Task<IEnumerable<HospitalProviderReadDto>> GetAllAsync();

        // Enable / Disable network participation
        Task ToggleNetworkAsync(int providerId, bool isNetwork);

        // Enable / Disable provider entirely
        Task ToggleActiveAsync(int providerId, bool isActive);
    }
}
