namespace HealthInsuranceApi.DTOs.Customer
{
    public class CustomerCreateDto
    {
        /// <summary>
        /// Full Name of the Customer
        /// </summary>

        public string UserId { get; set; } = string.Empty;  // Approved customer user
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string NomineeName { get; set; } = string.Empty;
    }
}
