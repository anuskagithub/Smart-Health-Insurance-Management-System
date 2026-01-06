using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.ClaimsOfficer;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IClaimsOfficerService
    {
        // Review submitted claims
        Task<IEnumerable<ClaimReadDto>> GetClaimsForReviewAsync();

        // Lifecycle transitions
        Task MoveToReviewAsync(int claimId);

        Task ApproveClaimAsync(int claimId, string remarks);

        Task RejectClaimAsync(int claimId, string remarks);

        Task MarkAsPaidAsync(int claimId);

        Task AddClaimOfficerAsync(ClaimsOfficerCreateDto dto, string userId);

        Task UpdateAsync(int officerId, ClaimsOfficerUpdateDto dto);

        Task<IEnumerable<ClaimsOfficerReadDto>> GetAllAsync();

        Task<IEnumerable<ClaimReadDto>> GetClaimsforClaimsOfficerAsync();
    }
}

