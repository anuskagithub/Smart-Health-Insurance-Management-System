using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.HospitalProvider;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Services.Implementations
{
    public class HospitalProviderService : IHospitalProviderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IClaimService _claimService;

        public HospitalProviderService(
            ApplicationDbContext context,
            IClaimService claimService)
        {
            _context = context;
            _claimService = claimService;
        }

        // =====================================================
        // PROVIDER ACTIONS (ROLE: HospitalProvider)
        // =====================================================

        // ---------------------------------------
        // SUBMIT CLAIM WITH NOTES
        // ---------------------------------------
        public async Task SubmitClaimAsync(
            ClaimCreateDto dto,
            string hospitalProviderUserId)
        {
            var provider = await _context.HospitalProviders
                .FirstOrDefaultAsync(h => h.UserId == hospitalProviderUserId)
                ?? throw new Exception("Hospital provider not found");

            // Validate coverage & policy status
            await _claimService.ValidateAndAllowClaimAsync(
                dto.PolicyId,
                dto.ClaimAmount);

            var claim = new Claim
            {
                PolicyId = dto.PolicyId,
                HospitalProviderId = provider.HospitalProviderId,
                ClaimAmount = dto.ClaimAmount,
                Remarks = dto.Remarks,   // ✅ NOTES
                Status = "Submitted",
                SubmittedOn = DateTime.UtcNow
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // VIEW PROVIDER CLAIMS
        // ---------------------------------------
        public async Task<IEnumerable<ClaimReadDto>>
            GetClaimsByProviderAsync(string hospitalProviderUserId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.HospitalProvider)
                .Where(c => c.HospitalProvider.UserId == hospitalProviderUserId)
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
                .ToListAsync();
        }

        // =====================================================
        // ADMIN MANAGEMENT (ROLE: Admin)
        // =====================================================

        // ---------------------------------------
        // CREATE HOSPITAL / PROVIDER
        // ---------------------------------------
        public async Task CreateAsync(HospitalProviderCreateDto dto, string userId)
        {
            var exists = await _context.HospitalProviders
                .AnyAsync(c => c.UserId == userId);

            if (exists)
                throw new Exception("Hospital profile already exists");

            var provider = new HospitalProvider
            {
                UserId = userId,
                ProviderName = dto.ProviderName,
                ProviderType = dto.ProviderType,
                City = dto.City,
                IsNetworkProvider = dto.IsNetworkProvider,
                IsActive = true

            };

            _context.HospitalProviders.Add(provider);
            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // UPDATE HOSPITAL / PROVIDER
        // ---------------------------------------
        public async Task UpdateAsync(int providerId,
            HospitalProviderUpdateDto dto)
        {
            var provider = await _context.HospitalProviders
                .FindAsync(providerId)
                ?? throw new Exception("Hospital/Provider not found");

            provider.ProviderName = dto.ProviderName;
            provider.ProviderType = dto.ProviderType;
            provider.City = dto.City;
            provider.IsNetworkProvider = dto.IsNetworkProvider;
            provider.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // VIEW ALL HOSPITALS / PROVIDERS
        // ---------------------------------------
        public async Task<IEnumerable<HospitalProviderReadDto>>
            GetAllAsync()
        {
            return await _context.HospitalProviders
                .Select(p => new HospitalProviderReadDto
                {
                    HospitalProviderId = p.HospitalProviderId,
                    ProviderName = p.ProviderName,
                    ProviderType = p.ProviderType,
                    City = p.City,
                    IsNetworkProvider = p.IsNetworkProvider,
                    IsActive = p.IsActive
                })
                .ToListAsync();
        }

        // ---------------------------------------
        // TOGGLE NETWORK STATUS
        // ---------------------------------------
        public async Task ToggleNetworkAsync(
            int providerId,
            bool isNetwork)
        {
            var provider = await _context.HospitalProviders
                .FindAsync(providerId)
                ?? throw new Exception("Provider not found");

            provider.IsNetworkProvider = isNetwork;
            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // ENABLE / DISABLE PROVIDER
        // ---------------------------------------
        public async Task ToggleActiveAsync(
            int providerId,
            bool isActive)
        {
            var provider = await _context.HospitalProviders
                .FindAsync(providerId)
                ?? throw new Exception("Provider not found");

            provider.IsActive = isActive;
            await _context.SaveChangesAsync();
        }
    }
}
