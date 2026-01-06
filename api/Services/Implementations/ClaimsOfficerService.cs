using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.ClaimsOfficer;
using HealthInsuranceApi.DTOs.HospitalProvider;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Services.Implementations
{
    public class ClaimsOfficerService : IClaimsOfficerService
    {
        private readonly ApplicationDbContext _context;

        public ClaimsOfficerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ----------------------------------------
        // REVIEW CLAIMS
        // ----------------------------------------
        public async Task<IEnumerable<ClaimReadDto>> GetClaimsForReviewAsync()
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.HospitalProvider)
                .Where(c => c.Status == "Submitted")
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
        // SUBMITTED → IN REVIEW
        // ----------------------------------------
        public async Task MoveToReviewAsync(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId)
                ?? throw new Exception("Claim not found");

            if (claim.Status != "Submitted")
                throw new Exception("Only submitted claims can be reviewed");

            claim.Status = "InReview";
            await _context.SaveChangesAsync();
        }

        // ----------------------------------------
        // IN REVIEW → APPROVED
        // ----------------------------------------
        public async Task ApproveClaimAsync(int claimId, string remarks)
        {
            var claim = await _context.Claims.FindAsync(claimId)
                ?? throw new Exception("Claim not found");

            if (claim.Status != "InReview")
                throw new Exception("Only in-review claims can be approved");

            //COVERAGE DEDUCTION
            if (claim.ClaimAmount > claim.Policy.CoverageRemaining)
                throw new Exception("Coverage exceeded");

            claim.Policy.CoverageRemaining -= claim.ClaimAmount;

            claim.Status = "Approved";
            claim.Remarks = remarks;

            await _context.SaveChangesAsync();
        }

        // ----------------------------------------
        // IN REVIEW → REJECTED
        // ----------------------------------------
        public async Task RejectClaimAsync(int claimId, string remarks)
        {
            var claim = await _context.Claims.FindAsync(claimId)
                ?? throw new Exception("Claim not found");

            if (claim.Status != "InReview")
                throw new Exception("Only in-review claims can be rejected");

            claim.Status = "Rejected";
            claim.Remarks = remarks;

            await _context.SaveChangesAsync();
        }

        // ----------------------------------------
        // APPROVED → PAID
        // ----------------------------------------
        public async Task MarkAsPaidAsync(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId)
                ?? throw new Exception("Claim not found");

            if (claim.Status != "Approved")
                throw new Exception("Only approved claims can be paid");

            claim.Status = "Paid";
            await _context.SaveChangesAsync();
        }


        public async Task AddClaimOfficerAsync(ClaimsOfficerCreateDto dto, string userId)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(userId))
                throw new Exception("UserId is required");

            if (string.IsNullOrWhiteSpace(dto.EmployeeCode))
                throw new Exception("EmployeeCode is required");

            if (string.IsNullOrWhiteSpace(dto.Department))
                throw new Exception("Department is required");

            // Check duplicate EmployeeCode
            bool exists = await _context.ClaimsOfficerProfiles
                .AnyAsync(c => c.EmployeeCode == dto.EmployeeCode);

            if (exists)
                throw new Exception("Claims Officer with this EmployeeCode already exists");

            var profile = new ClaimsOfficerProfile
            {
                UserId = userId,
                EmployeeCode = dto.EmployeeCode,
                Department = dto.Department,
                ApprovedClaimsCount = 0,
                IsActive = true
            };

            _context.ClaimsOfficerProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int profileId, ClaimsOfficerUpdateDto dto)
        {
            var claimsofficer = await _context.ClaimsOfficerProfiles
                .FindAsync(profileId)
                ?? throw new Exception("Claims officer not found");

            claimsofficer.Department = dto.Department;
            claimsofficer.User.FullName = dto.FullName;
            claimsofficer.User.Address= dto.Address;

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<ClaimsOfficerReadDto>> GetAllAsync()
        {
            return await _context.ClaimsOfficerProfiles
                .Select(p => new ClaimsOfficerReadDto
                {
                    ApprovedClaimsCount = p.ApprovedClaimsCount,
                    ClaimsOfficerProfileId = p.ClaimsOfficerProfileId,
                    Department = p.Department,
                    EmployeeCode = p.EmployeeCode,
                    IsActive = p.IsActive,
                    FullName = p.User.FullName,
                    Address = p.User.Address
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ClaimReadDto>> GetClaimsforClaimsOfficerAsync()
        {
            return await _context.Claims
                .Where(c=> c.Status.Equals("Hospital Reviewed"))
                .Select(c => new ClaimReadDto
                {
                    ClaimId = c.ClaimId,
                    ClaimAmount = c.ClaimAmount,
                    PolicyId = c.PolicyId,
                    ProviderName = c.HospitalProvider.ProviderName,
                    PolicyNumber = c.Policy.PolicyNumber,
                    Remarks = c.Remarks,
                    Status = c.Status,
                    SubmittedOn = c.SubmittedOn,
                    TreatmentHistory = c.TreatmentHistory
                })
                .ToListAsync();
        }

    }
}
