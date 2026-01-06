namespace HealthInsuranceApi.DTOs.Admin
{
    public class AdminUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsApproved { get; set; }
        public DateTime RegisteredOn { get; set; }
        public IList<string> Roles { get; set; }
    }
}
