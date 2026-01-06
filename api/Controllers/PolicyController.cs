using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly ICustomerService _customerService;

        public PolicyController(IPolicyService policyService, ICustomerService customerService)
        {
            _policyService = policyService;
            _customerService = customerService;
        }

        // --------------------------------------------------
        // ENROLL POLICY (INSURANCE AGENT)
        // --------------------------------------------------
        [Authorize(Roles = "Customer,InsuranceAgent")]
        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollPolicy(
            PolicyCreateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customerprofile = await _customerService.GetCustomerProfileId(userId);
            dto.CustomerProfileId = customerprofile.CustomerProfileId;

            var policyId = await _policyService.EnrollPolicyAsync(dto);
            return Ok(new
            {

                message = "Policy enrolled successfully",
                policyId

            });
        }

        // --------------------------------------------------
        // RENEW POLICY (INSURANCE AGENT)
        // --------------------------------------------------
        [Authorize(Roles = "Customer,InsuranceAgent")]
        [HttpPut("{policyId}/renew")]
        public async Task<IActionResult> RenewPolicy(
            int policyId)
        {
            await _policyService.RenewPolicyAsync(policyId);
            return Ok(new
            {
                message = "Policy renewed successfully"
            });
        }

        // --------------------------------------------------
        // SUSPEND POLICY (ADMIN)
        // --------------------------------------------------
        [Authorize(Roles = "Admin")]
        [HttpPut("{policyId}/suspend")]
        public async Task<IActionResult> SuspendPolicy(
            int policyId)
        {
            await _policyService.SuspendPolicyAsync(policyId);
            return Ok(new
            {
                message = "Policy suspended successfully"
            });
        }

        // --------------------------------------------------
        // VIEW ALL POLICIES (ADMIN / AGENT)
        // --------------------------------------------------
        [Authorize(Roles = "Admin,InsuranceAgent")]
        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            var policies = await _policyService.GetPoliciesAsync();
            return Ok(policies);
        }

        // GET POLICIES by claimID
        // --------------------------------------------------
        //[Authorize(Roles = "Admin,InsuranceAgent")]
        [HttpGet("PolicyById")]
        public async Task<IActionResult> GetPolicyByID(int policyId)
        {
            var policy = await _policyService.GetPolicyByIdAsync(policyId);

            if (policy == null)
                return NotFound($"Policy with Id {policyId} not found");

            return Ok(policy);
        }

        [HttpGet("PolicyTypeStatusReport")]
        public async Task<IActionResult> PolicyTypeStatusReportAsync()
        {
            var policies = await _policyService.GetPolicyTypeStatusReportAsync();
            return Ok(policies);
        }
    }
}
