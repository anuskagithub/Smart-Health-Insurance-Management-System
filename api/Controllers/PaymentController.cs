using HealthInsuranceApi.DTOs.Payment;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // =====================================================
        // CUSTOMER → PAY PREMIUM
        // =====================================================
        // Role: Customer
        // =====================================================
        [HttpPost("pay-premium")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PayPremium(PaymentCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Service now returns PolicyId
            var policyId = await _paymentService.PayPremiumAsync(dto, userId);

            return Ok(new
            {
                policyId = policyId,
                message = dto.IsGatewayConfirmed
                    ? "Premium payment completed successfully"
                    : "Payment initiated. Awaiting gateway confirmation"
            });
        }

        // =====================================================
        // CLAIMS OFFICER → PAY CLAIM (PAYOUT)
        // =====================================================
        // Role: ClaimsOfficer
        // =====================================================
        [HttpPut("claim-payout/{claimId}")]
        [Authorize(Roles = "ClaimsOfficer")]
        public async Task<IActionResult> MakeClaimPayout(int claimId)
        {
            await _paymentService.MakeClaimPayoutAsync(claimId);

            return Ok(new
            {
                message = "Claim payout processed successfully"
            });
        }

        // =====================================================
        // VIEW PAYMENTS FOR A POLICY
        // =====================================================
        // Roles: Admin, Customer
        // =====================================================
        [HttpGet("policy/{policyId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetPaymentsByPolicy(
            int policyId)
        {
            var payments =
                await _paymentService.GetPaymentsByPolicyAsync(policyId);

            return Ok(payments);
        }
    }
}
