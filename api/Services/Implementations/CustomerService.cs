using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.InsurancePlan;
using HealthInsuranceApi.DTOs.Payment;
using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IClaimService _claimService;

        public CustomerService(
            ApplicationDbContext context,
            IClaimService claimService)
        {
            _context = context;
            _claimService = claimService;
        }

        // ---------------------------------------
        // VIEW POLICIES
        // ---------------------------------------
        public async Task<IEnumerable<PolicyReadDto>>
            GetMyPoliciesAsync(string customerUserId)
        {
            return await _context.Policies
                .Include(p => p.InsurancePlan)
                .Include(p => p.CustomerProfile)
                .Where(p => p.CustomerProfile.UserId == customerUserId)
                .Select(p => new PolicyReadDto
                {
                    PolicyId = p.PolicyId,
                    PolicyNumber = p.PolicyNumber,
                    PlanName = p.InsurancePlan.PlanName,
                    Status = p.Status,
                    EndDate = p.EndDate,
                    CoverageRemaining = p.CoverageRemaining,
                    PremiumAmount = p.PremiumAmount

                })
                .ToListAsync();
        }

        // ---------------------------------------
        // PAY PREMIUM
        // ---------------------------------------
        public async Task PayPremiumAsync(
            PaymentCreateDto dto,
            string customerUserId)
        {
            var policy = await _context.Policies
                .Include(p => p.CustomerProfile)
                .FirstOrDefaultAsync(p =>
                    p.PolicyId == dto.PolicyId &&
                    p.CustomerProfile.UserId == customerUserId)
                ?? throw new Exception("Unauthorized policy access");

            //PREMIUM ENFORCEMENT
            if (dto.Amount != policy.PremiumAmount)
                throw new Exception(
                    $"Premium must be exactly {policy.PremiumAmount}");

            var payment = new Payment
            {
                PolicyId = dto.PolicyId,
                Amount = dto.Amount,
                PaymentType = "Premium",
                Status = "Completed"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // SUBMIT CLAIM WITH NOTES
        // ---------------------------------------
        public async Task SubmitClaimAsync(
            ClaimCreateDto dto,
            string customerUserId)
        {
            // Ensure policy belongs to customer
            var policy = await _context.Policies
                .Include(p => p.CustomerProfile)
                .FirstOrDefaultAsync(p =>
                    p.PolicyId == dto.PolicyId &&
                    p.CustomerProfile.UserId == customerUserId)
                ?? throw new Exception("Unauthorized policy access");

            // Validate coverage & policy status
            await _claimService.ValidateAndAllowClaimAsync(
                dto.PolicyId,
                dto.ClaimAmount);

            var claim = new Claim
            {
                PolicyId = dto.PolicyId,
                HospitalProviderId = dto.HospitalProviderId,
                ClaimAmount = dto.ClaimAmount,
                Remarks = dto.Remarks,   // ✅ NOTES
                Status = "Submitted"
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // TRACK CLAIMS
        // ---------------------------------------
        public async Task<IEnumerable<ClaimReadDto>>
            GetMyClaimsAsync(string customerUserId)
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
                    SubmittedOn = c.SubmittedOn,
                    ClaimOfficerComment = c.ClaimOfficerComment,
                    Remarks = c.Remarks,
                    TreatmentHistory = c.TreatmentHistory
                })
                .ToListAsync();
        }

        public async Task<ClaimReadDto?> GetMyClaimByIdAsync(
            int claimId,
            string customerUserId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Include(c => c.HospitalProvider)
                .Where(c =>
                    c.ClaimId == claimId &&
                    c.Policy.CustomerProfile.UserId == customerUserId)
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

        public async Task<IEnumerable<InsurancePlanReadDto>> GetAvailablePlansAsync()
        {
            return await _context.InsurancePlans
                .Where(p => p.IsActive) // optional but recommended
                .Select(p => new InsurancePlanReadDto
                {
                    InsurancePlanId = p.InsurancePlanId,
                    PlanName = p.PlanName,
                    PlanType = p.PlanType,
                    PremiumAmount = p.PremiumAmount,
                    CoverageAmount = p.CoverageAmount,
                    DurationInMonths = p.DurationInMonths
                })
                .ToListAsync();
        }


        public async Task CreateCustomerProfileAsync(CustomerCreateDto dto, string userId)
        {
            try
            {

                // Prevent duplicate profiles
                var exists = await _context.CustomerProfiles
                    .AnyAsync(c => c.UserId == userId);

                if (exists)
                    throw new Exception("Customer profile already exists");

                var customer = new CustomerProfile
                {
                    UserId = userId,
                    FullName = dto.FullName,
                    DateOfBirth = dto.DateOfBirth,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    NomineeName = dto.NomineeName,
                    CreatedOn = DateTime.UtcNow
                };

                _context.CustomerProfiles.Add(customer);
                await _context.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                throw new Exception("Error creating customer profile: " + ex.Message);
            }

        }

        public async Task<CustomerReadDto> GetCustomerProfileId(string userId)
        {
            return await _context.CustomerProfiles
                .Where(p => p.UserId == userId)
                .Select(p => new CustomerReadDto
                {
                    CustomerProfileId = p.CustomerProfileId,
                    FullName = p.FullName,
                    PhoneNumber = p.PhoneNumber
                }).FirstOrDefaultAsync();

        }
    }




}
