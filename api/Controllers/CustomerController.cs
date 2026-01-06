using HealthInsuranceApi.DTOs.Claim;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.Payment;
using HealthInsuranceApi.Services.Implementations;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IHospitalProviderService _hospitalService;

        public CustomerController(ICustomerService customerService, IHospitalProviderService hospitalService)
        {
            _customerService = customerService;
            _hospitalService = hospitalService;
        }

        // ----------------------------------------
        // VIEW MY POLICIES
        // ----------------------------------------
        [HttpGet("policies")]
        public async Task<IActionResult> GetMyPolicies()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var policies = await _customerService
                .GetMyPoliciesAsync(userId);

            return Ok(policies);
        }

        // ----------------------------------------
        // PAY PREMIUM
        // ----------------------------------------
        [HttpPost("pay-premium")]
        public async Task<IActionResult> PayPremium(
            PaymentCreateDto dto)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _customerService.PayPremiumAsync(dto, userId);

            return Ok(new { message = "Premium payment successful" });
        }

        // ----------------------------------------
        // SUBMIT CLAIM WITH NOTES
        // ----------------------------------------
        [HttpPost("submit-claim")]
        public async Task<IActionResult> SubmitClaim(
            ClaimCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _customerService
                .SubmitClaimAsync(dto, userId);

            return Ok(new
            {
                message = "Claim submitted successfully with notes"
            });
        }

        // ----------------------------------------
        // TRACK MY CLAIMS
        // ----------------------------------------
        [HttpGet("claims")]
        public async Task<IActionResult> GetMyClaims()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claims = await _customerService
                .GetMyClaimsAsync(userId);

            return Ok(claims);
        }

        // ----------------------------------------
        // GET CLAIM DETAILS
        // ----------------------------------------
        [HttpGet("claims/{claimId}")]
        public async Task<IActionResult> GetMyClaimById(int claimId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var claim = await _customerService
                .GetMyClaimByIdAsync(claimId, userId);

            if (claim == null)
                return NotFound(new { message = "Claim not found" });

            return Ok(claim);
        }

        [HttpGet("plans")]
        public async Task<IActionResult> GetAvailablePlans()
        {
            var plans = await _customerService.GetAvailablePlansAsync();
            return Ok(plans);
        }


        [HttpPost("profile")]
        public async Task<IActionResult> CreateCustomerProfile(CustomerCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _customerService.CreateCustomerProfileAsync(dto, userId);

            return Ok(new
            {
                message = "Customer profile created successfully"
            });
        }

        [HttpGet("hospital-providers")]
        public async Task<IActionResult> GetAllHospitalProviders()
        {
            return Ok(await _hospitalService.GetAllAsync());
        }

    }
}
