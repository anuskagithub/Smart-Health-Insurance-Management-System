using HealthInsuranceApi.DTOs.Reports;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IReportService
    {
        // =====================================================
        // POLICY REPORTS
        // =====================================================

        // Policies grouped by plan type and policy status
        Task<IEnumerable<PoliciesByTypeStatusReportDto>>
            GetPoliciesByTypeAndStatusAsync();

        // =====================================================
        // CLAIM REPORTS
        // =====================================================

        // Claims grouped by status, total amount, and hospital
        Task<IEnumerable<ClaimsStatusAmountHospitalReportDto>>
            GetClaimsByStatusAmountAndHospitalAsync();

        // High-value claims above a given threshold
        Task<IEnumerable<HighValueClaimReportDto>>
            GetHighValueClaimsAsync(decimal thresholdAmount);

        // Hospital-wise claim statistics (count & amounts)
        Task<IEnumerable<HospitalWiseClaimStatsDto>>
            GetHospitalWiseClaimStatsAsync();

        // =====================================================
        // FINANCIAL REPORTS
        // =====================================================

        // Premium collected vs payout made
        Task<PremiumVsPayoutReportDto>
            GetPremiumVsPayoutAsync();

        // =====================================================
        // DASHBOARD SUMMARY
        // =====================================================

        // Consolidated admin dashboard data
        Task<DashboardReportDto>
            GetDashboardAsync();

        Task<IEnumerable<PlanWisePolicyClaimCountDto>> GetPlanWisePolicyClaimCountAsync();
    }


}

