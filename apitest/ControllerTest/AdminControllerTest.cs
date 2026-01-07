﻿using HealthInsuranceApi.Controllers;
using HealthInsuranceApi.DTOs.Admin;
using HealthInsuranceApi.DTOs.Agent;
using HealthInsuranceApi.DTOs.ClaimsOfficer;
using HealthInsuranceApi.DTOs.HospitalProvider;
using HealthInsuranceApi.DTOs.InsurancePlan;
using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.DTOs.Reports;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace HealthInsuranceApi.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminService> _adminService = new();
        private readonly Mock<IInsurancePlanService> _planService = new();
        private readonly Mock<IPolicyService> _policyService = new();
        private readonly Mock<IHospitalProviderService> _hospitalService = new();
        private readonly Mock<IReportService> _reportService = new();
        private readonly Mock<IAuthService> _authService = new();
        private readonly Mock<IAgentService> _agentService = new();
        private readonly Mock<IClaimsOfficerService> _claimsOfficerService = new();

        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _controller = new AdminController(
                _adminService.Object,
                _planService.Object,
                _policyService.Object,
                _hospitalService.Object,
                _reportService.Object,
                _authService.Object,
                _agentService.Object,
                _claimsOfficerService.Object
            );
        }

        // ================= USER =================

        [Fact]
        public async Task GetAllUsers_ReturnsOk()
        {
            _adminService
                .Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(new List<AdminUserDto>());

            var result = await _controller.GetAllUsers();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_ReturnsOk()
        {
            var dto = new AdminUserCreateDto();

            var result = await _controller.CreateUser(dto);

            Assert.IsType<OkObjectResult>(result);
            _adminService.Verify(x => x.CreateUserAsync(dto), Times.Once);
        }

        // ================= PLANS =================

        [Fact]
        public async Task GetPlans_ReturnsOk()
        {
            _planService
                .Setup(x => x.GetAllPlansAsync())
                .ReturnsAsync(new List<InsurancePlanReadDto>());

            var result = await _controller.GetPlans();

            Assert.IsType<OkObjectResult>(result);
        }

        // ================= POLICIES =================

        [Fact]
        public async Task GetAllPolicies_ReturnsOk()
        {
            _policyService
                .Setup(x => x.GetPoliciesAsync())
                .ReturnsAsync(new List<PolicyReadDto>());

            var result = await _controller.GetAllPolicies();

            Assert.IsType<OkObjectResult>(result);
        }

        // ================= HOSPITAL PROVIDERS =================

        [Fact]
        public async Task GetAllHospitalProviders_ReturnsOk()
        {
            _hospitalService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<HospitalProviderReadDto>());

            var result = await _controller.GetAllHospitalProviders();

            Assert.IsType<OkObjectResult>(result);
        }

        // ================= REPORTS =================


        // ================= AGENTS =================

        [Fact]
        public async Task GetAllAgents_ReturnsOk()
        {
            _agentService
                .Setup(x => x.GetAllAgentAsync())
                .ReturnsAsync(new List<AgentReadDto>());

            var result = await _controller.GetAllAgents();

            Assert.IsType<OkObjectResult>(result);
        }

        // ================= CLAIM OFFICERS =================

        [Fact]
        public async Task GetAllClaimsOfficer_ReturnsOk()
        {
            _claimsOfficerService
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<ClaimsOfficerReadDto>());

            var result = await _controller.GetAllCliaimsOfficer();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_NullDto_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateUser(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async Task UpdatePlan_PlanNotFound_ReturnsNotFound()
        {
            _planService
                .Setup(x => x.UpdatePlanAsync(99, It.IsAny<InsurancePlanUpdateDto>()))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.UpdatePlan(99, new InsurancePlanUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task GetAllUsers_ServiceThrowsException_Returns500()
        {
            _adminService
                .Setup(x => x.GetAllUsersAsync())
                .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.GetAllUsers();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateUser_InvalidModel_ServiceNotCalled()
        {
            _controller.ModelState.AddModelError("Email", "Required");

            await _controller.CreateUser(new AdminUserCreateDto());

            _adminService.Verify(x => x.CreateUserAsync(It.IsAny<AdminUserCreateDto>()), Times.Never);
        }


    }
}
