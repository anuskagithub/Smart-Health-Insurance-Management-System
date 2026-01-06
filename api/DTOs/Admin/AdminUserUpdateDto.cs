namespace HealthInsuranceApi.DTOs.Admin
{
    public class AdminUserUpdateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Single role only (business rule)
        public string Role { get; set; }
    }
}
