using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Auth;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthInsuranceApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsApproved = false,
                RegisteredOn = DateTime.UtcNow,
                FullName = dto.FullName,
                Address = dto.Address
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception("User registration failed");

            return user.Id;

            /*var customerProfile = new CustomerProfile
            {
                UserId = user.Id,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                IsActive = true
            };

            _context.CustomerProfiles.Add(customerProfile);
            await _context.SaveChangesAsync();*/
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == dto.Username);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!user.IsApproved)
                throw new UnauthorizedAccessException("User not approved by admin");

            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid)
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id),
                new System.Security.Claims.Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            claims.AddRange(roles.Select(r => new System.Security.Claims.Claim(ClaimTypes.Role, r)));

            var jwtKey = _config["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key configuration is missing.");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["JwtSettings:TokenExpiryMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Roles = roles.ToList(),
                Expiry = token.ValidTo
            };
        }
    }
}

