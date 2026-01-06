using HealthInsuranceApi.Services.Implementations;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ClaimsOfficer")]
    public class ClaimsOfficerController : ControllerBase
    {
        private readonly IClaimsOfficerService _claimsOfficerService;

        public ClaimsOfficerController(
            IClaimsOfficerService claimsOfficerService)
        {
            _claimsOfficerService = claimsOfficerService;
        }

        // ----------------------------------------------------
        // VIEW CLAIMS PENDING REVIEW (SUBMITTED)
        // ----------------------------------------------------
        [HttpGet("review-queue")]
        public async Task<IActionResult> GetClaimsForReview()
        {
            var claims = await _claimsOfficerService
                .GetClaimsForReviewAsync();

            return Ok(claims);
        }

        // ----------------------------------------------------
        // SUBMITTED → IN REVIEW
        // ----------------------------------------------------
        [HttpPut("{claimId}/move-to-review")]
        public async Task<IActionResult> MoveToReview(int claimId)
        {
            await _claimsOfficerService
                .MoveToReviewAsync(claimId);

            return Ok(new
            {
                message = "Claim moved to review"
            });
        }

        // ----------------------------------------------------
        // IN REVIEW → APPROVED
        // (Coverage validation & deduction handled in service)
        // ----------------------------------------------------
        [HttpPut("{claimId}/approve")]
        public async Task<IActionResult> ApproveClaim(
            int claimId,
            [FromQuery] string remarks)
        {
            await _claimsOfficerService
                .ApproveClaimAsync(claimId, remarks);

            return Ok(new
            {
                message = "Claim approved successfully"
            });
        }

        // ----------------------------------------------------
        // IN REVIEW → REJECTED
        // ----------------------------------------------------
        [HttpPut("{claimId}/reject")]
        public async Task<IActionResult> RejectClaim(
            int claimId,
            [FromQuery] string remarks)
        {
            await _claimsOfficerService
                .RejectClaimAsync(claimId, remarks);

            return Ok(new
            {
                message = "Claim rejected successfully"
            });
        }

        // ----------------------------------------------------
        // APPROVED → PAID
        // ----------------------------------------------------
        [HttpPut("{claimId}/mark-paid")]
        public async Task<IActionResult> MarkAsPaid(int claimId)
        {
            await _claimsOfficerService
                .MarkAsPaidAsync(claimId);

            return Ok(new
            {
                message = "Claim marked as paid"
            });
        }

        [HttpGet("ClaimsforClaimsOfficer")]
        public async Task<IActionResult> GetClaimsforClaimsOfficer()
        {
            //var providerUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claims = await _claimsOfficerService.GetClaimsforClaimsOfficerAsync();

            return Ok(claims);
        }
    }
}
