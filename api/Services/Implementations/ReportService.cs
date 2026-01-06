using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Reports;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // POLICY REPORTS
        // =====================================================

        // Policies grouped by plan type and status
        public async Task<IEnumerable<PoliciesByTypeStatusReportDto>>
            GetPoliciesByTypeAndStatusAsync()
        {
            return await _context.Policies
                .Include(p => p.InsurancePlan)
                .GroupBy(p => new
                {
                    p.InsurancePlan.PlanType,
                    p.Status
                })
                .Select(g => new PoliciesByTypeStatusReportDto
                {
                    PlanType = g.Key.PlanType,
                    PolicyStatus = g.Key.Status,
                    PolicyCount = g.Count()
                })
                .ToListAsync();
        }

        // =====================================================
        // CLAIM REPORTS
        // =====================================================

        // Claims grouped by status, amount, and hospital
        public async Task<IEnumerable<ClaimsStatusAmountHospitalReportDto>>
            GetClaimsByStatusAmountAndHospitalAsync()
        {
            return await _context.Claims
                .Include(c => c.HospitalProvider)
                .GroupBy(c => new
                {
                    c.Status,
                    c.HospitalProvider.ProviderName
                })
                .Select(g => new ClaimsStatusAmountHospitalReportDto
                {
                    ClaimStatus = g.Key.Status,
                    ProviderName = g.Key.ProviderName,
                    ClaimCount = g.Count(),
                    TotalClaimAmount = g.Sum(x => x.ClaimAmount)
                })
                .ToListAsync();
        }

        // High-value claims
        public async Task<IEnumerable<HighValueClaimReportDto>>
            GetHighValueClaimsAsync(decimal thresholdAmount)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.ClaimAmount >= thresholdAmount)
                .Select(c => new HighValueClaimReportDto
                {
                    ClaimId = c.ClaimId,
                    PolicyNumber = c.Policy.PolicyNumber,
                    ClaimAmount = c.ClaimAmount,
                    Status = c.Status
                })
                .ToListAsync();
        }

        // Hospital-wise claim statistics
        public async Task<IEnumerable<HospitalWiseClaimStatsDto>>
            GetHospitalWiseClaimStatsAsync()
        {
            return await _context.Claims
                .Include(c => c.HospitalProvider)
                .GroupBy(c => new
                {
                    c.HospitalProviderId,
                    c.HospitalProvider.ProviderName
                })
                .Select(g => new HospitalWiseClaimStatsDto
                {
                    HospitalProviderId = g.Key.HospitalProviderId.HasValue? g.Key.HospitalProviderId.Value : 0,
                    HospitalName = g.Key.ProviderName,
                    TotalClaims = g.Count(),
                    TotalClaimAmount = g.Sum(x => x.ClaimAmount),
                    ApprovedClaimAmount = g
                        .Where(x => x.Status == "Approved")
                        .Sum(x => x.ClaimAmount)
                })
                .ToListAsync();
        }

        // =====================================================
        // FINANCIAL REPORTS
        // =====================================================

        // Premium vs payout
        public async Task<PremiumVsPayoutReportDto>
            GetPremiumVsPayoutAsync()
        {
            var totalPremium = await _context.Payments
                .Where(p => p.PaymentType == "Premium"
                         && p.Status == "Completed")
                .SumAsync(p => p.Amount);

            var totalPayout = await _context.Payments
                .Where(p => p.PaymentType == "ClaimPayout"
                         && p.Status == "Completed")
                .SumAsync(p => p.Amount);

            return new PremiumVsPayoutReportDto
            {
                TotalPremiumCollected = totalPremium,
                TotalClaimPayout = totalPayout
            };
        }

        // =====================================================
        // DASHBOARD SUMMARY
        // =====================================================

        public async Task<DashboardReportDto>
            GetDashboardAsync()
        {
            return new DashboardReportDto
            {
                // USERS
                TotalUsers = await _context.Users.CountAsync(),
                ApprovedUsers = await _context.Users
                    .CountAsync(u => u.IsApproved),

                // POLICIES
                TotalPolicies = await _context.Policies.CountAsync(),
                ActivePolicies = await _context.Policies
                    .CountAsync(p => p.Status == "Active"),
                ExpiredPolicies = await _context.Policies
                    .CountAsync(p => p.Status == "Expired"),
                SuspendedPolicies = await _context.Policies
                    .CountAsync(p => p.Status == "Suspended"),

                // CLAIMS
                TotalClaims = await _context.Claims.CountAsync(),
                SubmittedClaims = await _context.Claims
                    .CountAsync(c => c.Status == "Submitted"),
                InReviewClaims = await _context.Claims
                    .CountAsync(c => c.Status == "In Review"),
                ApprovedClaims = await _context.Claims
                    .CountAsync(c => c.Status == "Approved"),
                RejectedClaims = await _context.Claims
                    .CountAsync(c => c.Status == "Rejected"),
                PaidClaims = await _context.Claims
                    .CountAsync(c => c.Status == "Paid"),

                // FINANCIALS
                TotalPremiumCollected = await _context.Payments
                    .Where(p => p.PaymentType == "Premium"
                             && p.Status == "Completed")
                    .SumAsync(p => p.Amount),

                TotalClaimPayout = await _context.Payments
                    .Where(p => p.PaymentType == "ClaimPayout"
                             && p.Status == "Completed")
                    .SumAsync(p => p.Amount)
            };
        }
        public async Task<IEnumerable<PlanWisePolicyClaimCountDto>> GetPlanWisePolicyClaimCountAsync()
        {
            return await _context.Policies
        .Include(p => p.InsurancePlan)
        .GroupJoin(
            _context.Claims,
            policy => policy.PolicyNumber,
            claim => claim.Policy.PolicyNumber,
            (policy, claims) => new { policy, claims }
            )
            .GroupBy(x => x.policy.InsurancePlan.PlanName)
            .Select(g => new PlanWisePolicyClaimCountDto
            {
                PlanName = g.Key,
                PolicyCount = g.Select(x => x.policy.PolicyId).Distinct().Count(),
                ClaimCount = g.SelectMany(x => x.claims).Count()
            })
            .ToListAsync();
        }

    }
}
