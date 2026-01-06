namespace HealthInsuranceApi.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Expiry { get; set; }
    }
}
