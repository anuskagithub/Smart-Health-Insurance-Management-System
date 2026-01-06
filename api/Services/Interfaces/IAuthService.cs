using HealthInsuranceApi.DTOs.Auth;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
