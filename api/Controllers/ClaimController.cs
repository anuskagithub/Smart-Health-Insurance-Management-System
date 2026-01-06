using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.Services.Implementations;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly IPaymentService _paymentService;

        public ClaimController(IClaimService claimService, IPaymentService paymentService)
        {
            _claimService = claimService;
            _paymentService = paymentService;
        }

        // ----------------------------------------
        // ADMIN / OFFICER – VIEW ALL CLAIMS
        // ----------------------------------------
        [Authorize(Roles = "Admin,ClaimsOfficer")]
        [HttpGet]
        public async Task<IActionResult> GetAllClaims()
        {
            var claims = await _claimService.GetAllClaimsAsync();
            return Ok(claims);
        }

        // ----------------------------------------
        // CUSTOMER – VIEW MY CLAIMS
        // ----------------------------------------
        [Authorize(Roles = "Customer")]
        [HttpGet("my-claims")]
        public async Task<IActionResult> GetMyClaims()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claims = await _claimService
                .GetClaimsByCustomerAsync(userId);

            return Ok(claims);
        }


        // ----------------------------------------
        // CUSTOMER – SUBMIT CLAIM
        // ----------------------------------------
        [Authorize(Roles = "Customer")]
        [HttpPost("my-claims")]
        public async Task<IActionResult> SubmitClaim(ClaimCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _claimService.SubmitClaimAsync(dto, userId);
            return Ok(new { message = "Premium payment successful" });
        }


        // ----------------------------------------
        // VIEW CLAIM DETAILS (TRACKING)
        // ----------------------------------------
        [HttpGet("{claimId}")]
        public async Task<IActionResult> GetClaimById(int claimId)
        {
            var claim = await _claimService.GetClaimByIdAsync(claimId);

            if (claim == null)
                return NotFound(new { message = "Claim not found" });

            return Ok(claim);
        }

        // ----------------------------------------
        // TREATMENT DETAILS
        // ----------------------------------------
        [HttpGet("hospital-treatmnt")]
        public async Task<IActionResult> GetTreatment(int claimId)
        {
            var claim = await _claimService.GetTreatmentAsync(claimId);

            if (claim == null)
                return NotFound($"Calim with Id {claim} not found");

            return Ok(claim);
        }

        // ----------------------------------------
        // TREATMENT DETAILS
        // ----------------------------------------
        [HttpPut("hospital-treatmnt")]
        public async Task<IActionResult> SubmitTreatment(int claimId, string treatmentHistory)
        {
            await _claimService.SubmitTreatmentAsync(claimId, treatmentHistory);


            return Ok(new
            {
                message = "Treatment claim submitted successfully with notes"
            });
        }

        //[Authorize(Roles = "ClaimsOfficer")]
        [HttpPut("Claim-Approve-Reject")]
        public async Task<IActionResult> ClaimApproveReject(ClaimCreateDto dto, int claimId, string status)
        {
            await _claimService.ClaimApproveRejectAsync(dto, claimId, status);

            if (status.ToLower().Equals("approved"))
            {
                await _paymentService.MakeClaimPayoutAsync(claimId);
            }

            return Ok(new
            {
                message = $"Claim approval status : {status}"
            });
        }

        [HttpGet("ClaimsByStatusAmountandHospitalReport")]
        public async Task<IActionResult> GetClaimsByStatusAmountandHospital()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claims = await _claimService.GetClaimsByStatusAmountandHospitalAsync();

            return Ok(claims);
        }
    }
}
