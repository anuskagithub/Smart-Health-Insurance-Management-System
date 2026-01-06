using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using HealthInsuranceApi.Models;

namespace HealthInsuranceApi.Services.Implementations
{
    public class ClaimService : IClaimService
    {
        private readonly ApplicationDbContext _context;

        public ClaimService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----------------------------------------
        // VALIDATE CLAIM AGAINST POLICY
        // ----------------------------------------
        public async Task ValidateAndAllowClaimAsync(
            int policyId,
            decimal claimAmount)
        {
            var policy = await _context.Policies
                .Include(p => p.InsurancePlan)
                .FirstOrDefaultAsync(p => p.PolicyId == policyId)
                ?? throw new Exception("Policy not found");

            if (policy.Status != "Active")
                throw new Exception("Policy is not active");

            if (DateTime.UtcNow > policy.EndDate)
                throw new Exception("Policy has expired");

            if (claimAmount > policy.InsurancePlan.CoverageAmount)
                throw new Exception("Claim amount exceeds coverage limit");

        }

        // ----------------------------------------
        // CLAIM HISTORY & TRACKING
        // ----------------------------------------
        public async Task<IEnumerable<ClaimReadDto>> GetAllClaimsAsync()
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.HospitalProvider)
                .Select(c => new ClaimReadDto
                {
                    ClaimId = c.ClaimId,
                    PolicyNumber = c.Policy.PolicyNumber,
                    ProviderName = c.HospitalProvider.ProviderName,
                    ClaimAmount = c.ClaimAmount,
                    Status = c.Status,
                    SubmittedOn = c.SubmittedOn
                })
                .ToListAsync();
        }

        public async Task<ClaimReadDto?> GetClaimByIdAsync(int claimId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.HospitalProvider)
                .Where(c => c.ClaimId == claimId)
                .Select(c => new ClaimReadDto
                {
                    ClaimId = c.ClaimId,
                    PolicyNumber = c.Policy.PolicyNumber,
                    ProviderName = c.HospitalProvider.ProviderName,
                    ClaimAmount = c.ClaimAmount,
                    Status = c.Status,
                    SubmittedOn = c.SubmittedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClaimReadDto>>
            GetClaimsByCustomerAsync(string customerUserId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.HospitalProvider)
                .Where(c => c.Policy.CustomerProfile.UserId == customerUserId)
                .Select(c => new ClaimReadDto
                {
                    ClaimId = c.ClaimId,
                    PolicyNumber = c.Policy.PolicyNumber,
                    ProviderName = c.HospitalProvider.ProviderName,
                    ClaimAmount = c.ClaimAmount,
                    Status = c.Status,
                    SubmittedOn = c.SubmittedOn
                })
                .ToListAsync();
        }

        // ----------------------------------------
        // CUSTOMER – SUBMIT CLAIM
        // ----------------------------------------
        public async Task SubmitClaimAsync(ClaimCreateDto dto, string userId)
        {
            //if (dto.Amount != policy.PremiumAmount)
            //    throw new Exception(
            //        $"Premium must be exactly {policy.PremiumAmount}");

            var claim = new Claim
            {
                PolicyId = dto.PolicyId,
                HospitalProviderId = dto.HospitalProviderId,
                ClaimAmount = dto.ClaimAmount,
                Status = "Submitted",
                Remarks = dto.Remarks
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
        }

        // ----------------------------------------
        // Hospital Treatment
        // ----------------------------------------
        public async Task SubmitTreatmentAsync(int claimId, string treatmentHistory)
        {
            var claimtobeEdited = await _context.Claims.FindAsync(claimId);
            claimtobeEdited.TreatmentHistory = treatmentHistory;
            claimtobeEdited.Status = "Hospital Reviewed";

            await _context.SaveChangesAsync();
        }

        public async Task<ClaimReadDto?> GetTreatmentAsync(int claimId)
        {
            return await _context.Claims
               .Where(c => c.ClaimId == claimId)
                .Select(c => new ClaimReadDto
                {
                    ClaimId = c.ClaimId,
                    PolicyNumber = c.Policy.PolicyNumber,
                    ProviderName = c.HospitalProvider.ProviderName,
                    ClaimAmount = c.ClaimAmount,
                    Status = c.Status,
                    SubmittedOn = c.SubmittedOn,
                    ClaimOfficerComment = c.ClaimOfficerComment,
                    Remarks = c.Remarks,
                    TreatmentHistory = c.TreatmentHistory
                })
                .FirstOrDefaultAsync();
        }

        public async Task ClaimApproveRejectAsync(ClaimCreateDto dto, int claimId, string status)
        {
            var claimtobeEdited = await _context.Claims.FindAsync(claimId);
            claimtobeEdited.ClaimOfficerComment = dto.Remarks;
            claimtobeEdited.Status = status;

            //use dto for updating claim remailing amount is approved
            if (status.ToLower() == "approve")
            {
                var policy = await _context.Policies.FindAsync(claimtobeEdited.PolicyId)
            ?? throw new Exception("Policy not found");

                policy.CoverageRemaining = policy.CoverageRemaining - claimtobeEdited.ClaimAmount;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClaimsByStatusAmountHospitalDto>> GetClaimsByStatusAmountandHospitalAsync()
        {
            return await _context.Claims
                .Include(c => c.HospitalProvider)
                .GroupBy(c => new
                {
                    c.HospitalProvider.ProviderName,
                    c.Status
                }).Select(g => new ClaimsByStatusAmountHospitalDto
                {
                    ProviderName = g.Key.ProviderName,
                    Status = g.Key.Status,
                    ClaimCount = g.Count(),
                    TotalClaimAmount = g.Sum(x => x.ClaimAmount)
                }).ToListAsync();
        }
    }
}
    

