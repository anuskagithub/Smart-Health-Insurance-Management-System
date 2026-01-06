using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Hospital")]
    public class HospitalProviderController : ControllerBase
    {
        private readonly IHospitalProviderService _hospitalProviderService;

        public HospitalProviderController(
            IHospitalProviderService hospitalProviderService)
        {
            _hospitalProviderService = hospitalProviderService;
        }

        // ----------------------------------------
        // SUBMIT TREATMENT DETAILS / CLAIM WITH NOTES
        // ----------------------------------------
        [HttpPost("submit-claim")]
        public async Task<IActionResult> SubmitClaim(
            ClaimCreateDto dto)
        {
            var providerUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _hospitalProviderService
                .SubmitClaimAsync(dto, providerUserId);

            return Ok(new
            {
                message = "Treatment claim submitted successfully with notes"
            });
        }

        // ----------------------------------------
        // VIEW ALL CLAIMS SUBMITTED BY THIS PROVIDER
        // ----------------------------------------
        [HttpGet("claims")]
        public async Task<IActionResult> GetMyClaims()
        {
            var providerUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claims = await _hospitalProviderService
                .GetClaimsByProviderAsync(providerUserId);

            return Ok(claims);
        }

        // ----------------------------------------
        // VIEW SINGLE CLAIM (TRACK STATUS)
        // ----------------------------------------
        [HttpGet("claims/{claimId}")]
        public async Task<IActionResult> GetClaimById(int claimId)
        {
            var providerUserId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claims = await _hospitalProviderService
                .GetClaimsByProviderAsync(providerUserId);

            var claim = claims.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null)
                return NotFound(new
                {
                    message = "Claim not found or access denied"
                });

            return Ok(claim);
        }
    }
}
