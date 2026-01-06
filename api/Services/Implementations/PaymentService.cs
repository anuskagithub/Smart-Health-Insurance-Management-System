using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Payment;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthInsuranceApi.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> PayPremiumAsync(PaymentCreateDto dto,string userId)
        {
            Policy policy;

            // 🟢 NEW POLICY PURCHASE
            if (dto.PolicyId == 0)
            {
                var plan = await _context.InsurancePlans
                    .FirstOrDefaultAsync(p => p.InsurancePlanId == dto.InsurancePlanId)
                    ?? throw new Exception("Invalid insurance plan");

                policy = new Policy
                {
                    PolicyNumber = $"POL-{Guid.NewGuid().ToString()[..8]}",
                    InsurancePlanId = plan.InsurancePlanId,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddMonths(12),
                    Status = "Active",
                    PremiumAmount = plan.PremiumAmount,
                    CoverageRemaining = plan.CoverageAmount
                };

                _context.Policies.Add(policy);
                await _context.SaveChangesAsync();
            }
            else
            {
                // RENEWAL PAYMENT
                policy = await _context.Policies
                    .Include(p => p.CustomerProfile)
                    .FirstOrDefaultAsync(p =>
                        p.PolicyId == dto.PolicyId &&
                        p.CustomerProfile.UserId == userId)
                    ?? throw new Exception("Policy not found");
            }

            //CREATE PAYMENT
            var payment = new Payment
            {
                PolicyId = policy.PolicyId,
                Amount = dto.Amount > 0 ? dto.Amount : policy.PremiumAmount,
                PaymentType = "Premium",
                Status = dto.IsGatewayConfirmed ? "Completed" : "Pending",
                PaymentDate = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return policy.PolicyId;
        }

        public async Task MakeClaimPayoutAsync(int claimId)
        {
            var claim = await _context.Claims
                .Include(c => c.Policy)
                .FirstOrDefaultAsync(c => c.ClaimId == claimId)
                ?? throw new Exception("Claim not found");

            if (claim.Status != "Approved")
                throw new Exception("Only approved claims can be paid");

            var payment = new Payment
            {
                ClaimId = claimId,
                PolicyId = claim.PolicyId,
                Amount = claim.ClaimAmount,
                PaymentType = "ClaimPayout",
                Status = "Completed"
            };

            claim.Status = "Paid";

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PaymentReadDto>>
            GetPaymentsByPolicyAsync(int policyId)
        {
            return await _context.Payments
                .Where(p => p.PolicyId == policyId)
                .Select(p => new PaymentReadDto
                {
                    PaymentId = p.PaymentId,
                    PaymentType = p.PaymentType,
                    Amount = p.Amount,
                    Status = p.Status,
                    PaymentDate = p.PaymentDate
                })
                .ToListAsync();
        }
    }
}
