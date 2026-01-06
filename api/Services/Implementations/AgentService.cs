using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.HospitalProvider;
using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using HealthInsuranceApi.DTOs.Agent;

namespace HealthInsuranceApi.Services.Implementations
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;

        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(AgentCreateDto dto, string userId)
        {
            var exists = await _context.AgentProfiles
                .AnyAsync(c => c.UserId == userId);

            if (exists)
                throw new Exception("Agent profile already exists");

            var agent = new AgentProfile
            {
                UserId = userId,
                AgentCode = dto.AgentCode,
                CreatedOn = DateTime.UtcNow,
                Region = dto.Region,
                YearsOfExperience = dto.YearsOfExperience,
                IsActive = true
            };

            _context.AgentProfiles.Add(agent);
            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // UPDATE Agent
        // ---------------------------------------
        public async Task UpdateAsync(int ProfileId, AgentUpdateDto dto)
        {
            var agent = await _context.AgentProfiles
                .FindAsync(ProfileId)
                ?? throw new Exception("Agent not found");

            
            agent.YearsOfExperience = dto.YearsOfExperience;
            agent.Region = dto.Region;
            agent.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
        }

        // ---------------------------------------
        // VIEW ALL AGENTS
        // ---------------------------------------
        public async Task<IEnumerable<AgentReadDto>> GetAllAgentAsync()
        {
            return await _context.AgentProfiles
                .Select(p => new AgentReadDto
                {
                   
                     AgentCode = p.AgentCode,
                     YearsOfExperience = p.YearsOfExperience,
                     Region = p.Region,
                     AgentProfileId = p.AgentProfileId,
                     FullName=p.User.FullName,
                     Address= p.User.Address
                     
                     
                                      
                })
                .ToListAsync();
        }

        // ------------------------------------
        // AGENT ENROLLS CUSTOMER
        // ------------------------------------
        public async Task EnrollCustomerAsync(
            string agentUserId,
            CustomerCreateDto dto)
        {
            // Get agent profile
            var agentProfile = await _context.AgentProfiles
                .FirstOrDefaultAsync(a => a.UserId == agentUserId)
                ?? throw new Exception("Agent profile not found");

            // Ensure customer user exists & approved
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId && u.IsApproved)
                ?? throw new Exception("Customer user not approved by admin");

            // Prevent duplicate enrollment
            var alreadyExists = await _context.CustomerProfiles
                .AnyAsync(c => c.UserId == dto.UserId);

            if (alreadyExists)
                throw new Exception("Customer already enrolled");

            var customerProfile = new CustomerProfile
            {
                UserId = dto.UserId,
                AgentProfileId = agentProfile.AgentProfileId,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                IsActive = true
            };

            _context.CustomerProfiles.Add(customerProfile);
            await _context.SaveChangesAsync();
        }

        // ------------------------------------
        // AGENT CREATES POLICY
        // ------------------------------------
        public async Task CreatePolicyAsync(PolicyCreateDto dto)
        {
            var policy = new Policy
            {
                InsurancePlanId = dto.InsurancePlanId,
                CustomerProfileId = dto.CustomerProfileId,
                StartDate = dto.StartDate,
                EndDate = dto.StartDate.AddMonths(12),
                Status = "Active",
                PolicyNumber = Guid.NewGuid().ToString("N")[..10]
            };

            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PolicyReadDto>>
            GetPoliciesByAgentAsync(string agentUserId)
        {
            return await _context.Policies
                .Include(p => p.InsurancePlan)
                .Include(p => p.CustomerProfile)
                .ThenInclude(c => c.AgentProfile)
                .Where(p => p.CustomerProfile.AgentProfile.UserId == agentUserId)
                .Select(p => new PolicyReadDto
                {
                    PolicyId = p.PolicyId,
                    PolicyNumber = p.PolicyNumber,
                    PlanName = p.InsurancePlan.PlanName,
                    Status = p.Status,
                    EndDate = p.EndDate
                })
                .ToListAsync();
        }
    }
}
