using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // ----------------------------------
        // Policies by Type and Status
        // ----------------------------------
        [HttpGet("policies-by-type-status")]
        public async Task<IActionResult> GetPoliciesByTypeAndStatus()
        {
            var result = await _reportService
                .GetPoliciesByTypeAndStatusAsync();
            return Ok(result);
        }

        // ----------------------------------
        // Claims by Status, Amount & Hospital
        // ----------------------------------
        [HttpGet("claims-by-status-amount-hospital")]
        public async Task<IActionResult> GetClaimsByStatusAmountHospital()
        {
            var result = await _reportService
                .GetClaimsByStatusAmountAndHospitalAsync();
            return Ok(result);
        }

        // ----------------------------------
        // Premium vs Payout
        // ----------------------------------
        [HttpGet("premium-vs-payout")]
        public async Task<IActionResult> GetPremiumVsPayout()
        {
            var result = await _reportService.GetPremiumVsPayoutAsync();
            return Ok(result);
        }

        // ----------------------------------
        // High-Value Claims
        // ----------------------------------
        [HttpGet("high-value-claims")]
        public async Task<IActionResult> GetHighValueClaims(
            [FromQuery] decimal threshold = 100000)
        {
            var result = await _reportService
                .GetHighValueClaimsAsync(threshold);
            return Ok(result);
        }

        [HttpGet("Plan-Wise-PolicyClaimCount")]
        public async Task<IActionResult> GetPlanWisePolicyClaimCount()
        {
            var result = await _reportService.GetPlanWisePolicyClaimCountAsync();
            return Ok(result);
        }
    }
}
