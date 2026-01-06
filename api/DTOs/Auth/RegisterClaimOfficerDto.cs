using HealthInsuranceApi.DTOs.ClaimsOfficer;

namespace HealthInsuranceApi.DTOs.Auth
{
    public class RegisterClaimOfficerDto
    {
        public RegisterDto RegisterInfo { get; set; }
        public ClaimsOfficerCreateDto ClaimsOfficerCreateInfo { get; set; }
    }
}
