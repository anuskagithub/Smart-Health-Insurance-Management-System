namespace HealthInsuranceApi.DTOs.Reports
{
    public class DashboardReportDto
    {
        // -----------------------------
        // USER METRICS
        // -----------------------------
        public int TotalUsers { get; set; }
        public int ApprovedUsers { get; set; }

        // -----------------------------
        // POLICY METRICS
        // -----------------------------
        public int TotalPolicies { get; set; }
        public int ActivePolicies { get; set; }
        public int ExpiredPolicies { get; set; }
        public int SuspendedPolicies { get; set; }

        // -----------------------------
        // CLAIM METRICS
        // -----------------------------
        public int TotalClaims { get; set; }
        public int SubmittedClaims { get; set; }
        public int InReviewClaims { get; set; }
        public int ApprovedClaims { get; set; }
        public int RejectedClaims { get; set; }
        public int PaidClaims { get; set; }

        // -----------------------------
        // FINANCIAL METRICS
        // -----------------------------
        public decimal TotalPremiumCollected { get; set; }
        public decimal TotalClaimPayout { get; set; }
    }
}
