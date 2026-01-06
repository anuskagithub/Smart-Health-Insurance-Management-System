using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Admin;
using HealthInsuranceApi.DTOs.Agent;
using HealthInsuranceApi.DTOs.ClaimsOfficer;
using HealthInsuranceApi.DTOs.Customer;
using HealthInsuranceApi.DTOs.HospitalProvider;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthInsuranceApi.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Registered users retrieval
        public async Task<IEnumerable<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<AdminUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new AdminUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    PhoneNumber=user.PhoneNumber,
                    IsApproved = user.IsApproved,
                    RegisteredOn = user.RegisteredOn,
                    Roles = roles.ToList()
                });
            }

            return result;
        }

        // ===========================
        // CREATE USER (ADMIN)
        // ===========================
        public async Task CreateUserAsync(AdminUserCreateDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                IsApproved = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            // Ensure role exists
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));

            await _userManager.AddToRoleAsync(user, dto.Role);
        }

        // ===========================
        // UPDATE USER + ROLE
        // ===========================
        public async Task UpdateUserAsync(string userId, AdminUserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            // =========================
            // UPDATE BASIC DETAILS
            // =========================
            user.UserName = dto.UserName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new Exception(updateResult.Errors.First().Description);

            // =========================
            // UPDATE ROLE (SINGLE ROLE)
            // =========================
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Ensure role exists
            if (!await _roleManager.RoleExistsAsync(dto.Role))
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));

            await _userManager.AddToRoleAsync(user, dto.Role);
        }


        // ===========================
        // DELETE USER
        // ===========================
        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            await _userManager.DeleteAsync(user);
        }


        // =====================================================
        // USER APPROVAL
        // =====================================================
        public async Task ApproveUserAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new Exception("User not found");

            user.IsApproved = true;
            await _userManager.UpdateAsync(user);
        }

        // =====================================================
        // ROLE ASSIGNMENT (MULTI-ROLE SUPPORTED)
        // =====================================================
        public async Task AssignRolesAsync(
            string userId,
            List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            if (!user.IsApproved)
                throw new Exception("User must be approved first");

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(
                        new IdentityRole(role));
                }

                if (!await _userManager.IsInRoleAsync(user, role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        // =====================================================
        // CUSTOMER PROFILE CREATION
        // =====================================================
        public async Task CreateCustomerProfileAsync(
            string userId,
            CustomerCreateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            if (!user.IsApproved)
                throw new Exception("User not approved");

            var profileExists = await _context.CustomerProfiles
                .AnyAsync(c => c.UserId == userId);

            if (profileExists)
                throw new Exception("Customer profile already exists");

            var profile = new CustomerProfile
            {
                UserId = userId,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                NomineeName = dto.NomineeName,
                IsActive = true
            };

            _context.CustomerProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        // =====================================================
        // INSURANCE AGENT PROFILE CREATION
        // =====================================================
        public async Task CreateAgentProfileAsync(
            string userId,
            AgentCreateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var exists = await _context.AgentProfiles
                .AnyAsync(a => a.UserId == userId);

            if (exists)
                throw new Exception("Agent profile already exists");

            var profile = new AgentProfile
            {
                UserId = userId,
                AgentCode = dto.AgentCode,
                Region = dto.Region,
                IsActive = true
            };

            _context.AgentProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        // =====================================================
        // CLAIMS OFFICER PROFILE CREATION
        // =====================================================
        public async Task CreateClaimsOfficerProfileAsync(
            string userId,
            ClaimsOfficerCreateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var exists = await _context.ClaimsOfficerProfiles
                .AnyAsync(c => c.UserId == userId);

            if (exists)
                throw new Exception("Claims officer profile already exists");

            var profile = new ClaimsOfficerProfile
            {
                UserId = userId,
                EmployeeCode = dto.EmployeeCode,
                Department = dto.Department,
                IsActive = true
            };

            _context.ClaimsOfficerProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        // =====================================================
        // HOSPITAL / PROVIDER PROFILE CREATION
        // =====================================================
        public async Task CreateHospitalProfileAsync(string userId, HospitalProviderCreateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new Exception("User not found");

            var exists = await _context.HospitalProviders
                .AnyAsync(h => h.UserId == userId);

            if (exists)
                throw new Exception("Hospital/provider profile already exists");

            var profile = new HospitalProvider
            {
                UserId = userId,
                ProviderName = dto.ProviderName,
                ProviderType = dto.ProviderType,
                City = dto.City,
                IsNetworkProvider = dto.IsNetworkProvider,
                IsActive = true
            };

            _context.HospitalProviders.Add(profile);
            await _context.SaveChangesAsync();
        }
    }
}
