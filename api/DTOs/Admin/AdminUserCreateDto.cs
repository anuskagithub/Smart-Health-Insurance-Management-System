namespace HealthInsuranceApi.DTOs.Admin
{
    public class AdminUserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // SINGLE ROLE
    }
}
