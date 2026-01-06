using HealthInsuranceApi.DTOs.Customer;

namespace HealthInsuranceApi.DTOs.Auth
{
    public class CustomerRegistration
    {
        public RegisterDto RegisterInfo { get; set; }
        public CustomerCreateDto CustProfileInfo { get; set; }

    }
}
