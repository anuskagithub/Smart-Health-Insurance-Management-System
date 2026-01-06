using HealthInsuranceApi.DTOs.Admin;
using HealthInsuranceApi.DTOs.Agent;
using HealthInsuranceApi.DTOs.Auth;
using HealthInsuranceApi.DTOs.ClaimsOfficer;
using HealthInsuranceApi.DTOs.HospitalProvider;
using HealthInsuranceApi.DTOs.InsurancePlan;
using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.DTOs.Reports;
using HealthInsuranceApi.Services.Implementations;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthInsuranceApi.Controllers
{

    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    //[Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IInsurancePlanService _planService;
        private readonly IPolicyService _policyService;
        private readonly IHospitalProviderService _hospitalService;
        private readonly IReportService _reportService;
        private readonly IAuthService _authService;
        private readonly IAgentService _agentService;
        private readonly IClaimsOfficerService _claimsOfficerService;
        public AdminController(
            IAdminService adminService,
            IInsurancePlanService planService,
            IPolicyService policyService,
            IHospitalProviderService hospitalService,
            IReportService reportService,
            IAuthService authService,
            IAgentService agentService,
            IClaimsOfficerService claimsOfficerService)
        {
            _adminService = adminService;
            _planService = planService;
            _policyService = policyService;
            _hospitalService = hospitalService;
            _reportService = reportService;
            _authService = authService;
            _agentService = agentService;
            _claimsOfficerService = claimsOfficerService;
        }

        // =====================================================
        // A. USER & ROLE MANAGEMENT
        // =====================================================

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(AdminUserCreateDto dto)
        {
            await _adminService.CreateUserAsync(dto);
            return Ok(new { message = "User created successfully" });
        }

        [HttpPut("users/{userId}")]
        public async Task<IActionResult> UpdateUser(
    string userId,
    AdminUserUpdateDto dto)
        {
            await _adminService.UpdateUserAsync(userId, dto);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("users/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await _adminService.DeleteUserAsync(userId);
            return Ok(new { message = "User deleted successfully" });
        }



        [HttpPut("users/{userId}/approve")]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            await _adminService.ApproveUserAsync(userId);
            return Ok(new { message = "User approved" });
        }


        [HttpPut("users/{userId}/roles")]
        public async Task<IActionResult> AssignRoles(
            string userId,
            [FromBody] List<string> roles)
        {
            await _adminService.AssignRolesAsync(userId, roles);
            return Ok(new { message = "Roles assigned" });
        }

        // =====================================================
        // B. INSURANCE PLAN MANAGEMENT (CRUD)
        // =====================================================

        [HttpPost("plans")]
        public async Task<IActionResult> CreatePlan(
        [FromBody] InsurancePlanCreateDto dto)
        {
            await _planService.CreatePlanAsync(dto);
            return Ok(new { message = "Insurance plan created" });
        }

        [HttpPut("plans/{planId}")]
        public async Task<IActionResult> UpdatePlan(
        int planId,
        [FromBody] InsurancePlanUpdateDto dto)
        {
            await _planService.UpdatePlanAsync(planId, dto);
            return Ok(new { message = "Insurance plan updated" });
        }

        [HttpPatch("plans/{planId}/disable")]
        public async Task<IActionResult> DisablePlan(int planId)
        {
            await _planService.DisablePlanAsync(planId);
            return Ok(new { message = "Insurance plan disabled" });
        }

        [HttpPatch("plans/{planId}/status")]
        public async Task<IActionResult> TogglePlanStatus(
        int planId,
        [FromQuery] bool isActive)
        {
            await _planService.TogglePlanStatusAsync(planId, isActive);
            return Ok(new { message = "Plan status updated" });
        }

        [HttpGet("plans")]
        public async Task<IActionResult> GetPlans()
        {
            return Ok(await _planService.GetAllPlansAsync());
        }

        // =====================================================
        // C. POLICY MANAGEMENT
        // =====================================================

        [HttpPut("policies/{policyId}/suspend")]
        public async Task<IActionResult> SuspendPolicy(int policyId)
        {
            await _policyService.SuspendPolicyAsync(policyId);
            return Ok(new { message = "Policy suspended" });
        }

        [HttpGet("policies")]
        public async Task<IActionResult> GetAllPolicies()
        {
            return Ok(await _policyService.GetPoliciesAsync());
        }

        // =====================================================
        // D. HOSPITAL / PROVIDER MANAGEMENT
        // =====================================================

        [HttpPost("hospital-providers")]
        public async Task<IActionResult> CreateHospitalProvider(HospitalRegistration dto)
        {
            var userId = await _authService.RegisterAsync(dto.RegisterInfo);
            await _hospitalService.CreateAsync(dto.HospitalProviderInfo, userId);
            return Ok(new { message = "Hospital/Provider created" });

        }

        [HttpPut("hospital-providers/{id}")]
        public async Task<IActionResult> UpdateHospitalProvider(
            int id,
            HospitalProviderUpdateDto dto)
        {
            await _hospitalService.UpdateAsync(id, dto);
            return Ok(new { message = "Hospital/Provider updated" });
        }

        [HttpPut("hospital-providers/{id}/network")]
        public async Task<IActionResult> ToggleNetwork(
            int id,
            [FromQuery] bool isNetwork)
        {
            await _hospitalService.ToggleNetworkAsync(id, isNetwork);
            return Ok(new { message = "Network status updated" });
        }

        [HttpPut("hospital-providers/{id}/active")]
        public async Task<IActionResult> ToggleActive(
            int id,
            [FromQuery] bool isActive)
        {
            await _hospitalService.ToggleActiveAsync(id, isActive);
            return Ok(new { message = "Provider active status updated" });
        }

        [HttpGet("hospital-providers")]
        public async Task<IActionResult> GetAllHospitalProviders()
        {
            return Ok(await _hospitalService.GetAllAsync());
        }

        // =====================================================
        // E. REPORTING & DASHBOARDS
        // =====================================================

        [HttpGet("reports/hospital-claims")]
        public async Task<IActionResult> HospitalWiseClaimStats()
        {
            return Ok(await _reportService
                .GetHospitalWiseClaimStatsAsync());
        }

        [HttpGet("reports/dashboard")]
        public async Task<IActionResult> DashboardSummary()
        {
            return Ok(await _reportService.GetDashboardAsync());
        }


        //Agent

        [HttpPost("agents")]
        public async Task<IActionResult> CreateAgent(AgentRegistration dto)
        {
            var userId = await _authService.RegisterAsync(dto.RegisterInfo);
            await _agentService.CreateAsync(dto.AgentProfileInfo, userId);
            return Ok(new { message = "Hospital/Provider created" });

        }

        [HttpPut("agents/{id}")]
        public async Task<IActionResult> UpdateAgent(int id, AgentUpdateDto dto)
        {
            await _agentService.UpdateAsync(id, dto);
            return Ok(new { message = "Agent updated" });
        }

        [HttpGet("agents")]
        public async Task<IActionResult> GetAllAgents()
        {
            return Ok(await _agentService.GetAllAgentAsync());
        }

        //Claims Officer

        [HttpPost("claimofficer")]
        public async Task<IActionResult> RegisterClaimOfficer(RegisterClaimOfficerDto dto)
        {
            
            var userId = await _authService.RegisterAsync(dto.RegisterInfo);
            await _claimsOfficerService.AddClaimOfficerAsync(dto.ClaimsOfficerCreateInfo, userId);
            return Ok("ClaimsOfficer Created.");
        }

        [HttpPut("claimofficer/{id}")]
        public async Task<IActionResult> UpdateCliaimsOfficer(int id, ClaimsOfficerUpdateDto dto)
        {
            await _claimsOfficerService.UpdateAsync(id, dto);
            return Ok(new { message = "Hospital/Provider updated" });
        }

        [HttpGet("claimofficer")]
        public async Task<IActionResult> GetAllCliaimsOfficer()
        {
            return Ok(await _claimsOfficerService.GetAllAsync());
        }




    }


}