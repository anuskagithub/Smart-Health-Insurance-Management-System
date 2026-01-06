using HealthInsuranceApi.DTOs.Claim;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IClaimService
    {
        // -------------------------------
        // Validation
        // -------------------------------
        Task ValidateAndAllowClaimAsync(
            int policyId,
            decimal claimAmount);

        // -------------------------------
        // Tracking / History
        // -------------------------------
        Task<IEnumerable<ClaimReadDto>> GetAllClaimsAsync();

        Task<ClaimReadDto?> GetClaimByIdAsync(int claimId);

        Task<IEnumerable<ClaimReadDto>>
            GetClaimsByCustomerAsync(string customerUserId);

        Task SubmitClaimAsync(ClaimCreateDto dto, string userId);

        Task<ClaimReadDto?> GetTreatmentAsync(int claimId);

        Task ClaimApproveRejectAsync(ClaimCreateDto dto, int claimId, string status);

        Task SubmitTreatmentAsync(int claimId, string treatmentHistory);

        Task<IEnumerable<ClaimsByStatusAmountHospitalDto>> GetClaimsByStatusAmountandHospitalAsync();
    }
}
