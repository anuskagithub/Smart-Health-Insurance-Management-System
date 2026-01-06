namespace HealthInsuranceApi.DTOs.Customer
{
    public class CustomerReadDto
    {
        public int CustomerProfileId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
