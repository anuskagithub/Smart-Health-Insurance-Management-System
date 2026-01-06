using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthInsuranceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "InsuranceAgent")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        // ------------------------------------
        // ENROLL CUSTOMER
        // ------------------------------------
        [HttpPost("enroll-customer")]
        public async Task<IActionResult> EnrollCustomer(
            CustomerCreateDto dto)
        {
            var agentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _agentService.EnrollCustomerAsync(agentUserId, dto);

            return Ok(new { message = "Customer enrolled successfully" });
        }

        // ------------------------------------
        // CREATE POLICY
        // ------------------------------------
        [HttpPost("create-policy")]
        public async Task<IActionResult> CreatePolicy(
            PolicyCreateDto dto)
        {
            await _agentService.CreatePolicyAsync(dto);
            return Ok(new { message = "Policy created successfully" });
        }

        // ------------------------------------
        // GET AGENT POLICIES
        // ------------------------------------
        [HttpGet("policies")]
        public async Task<IActionResult> GetPolicies()
        {
            var agentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var policies = await _agentService
                .GetPoliciesByAgentAsync(agentUserId);

            return Ok(policies);
        }
    }
}
