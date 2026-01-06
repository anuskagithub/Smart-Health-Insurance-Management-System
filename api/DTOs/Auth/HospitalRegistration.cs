using HealthInsuranceApi.DTOs.HospitalProvider;

namespace HealthInsuranceApi.DTOs.Auth
{
    public class HospitalRegistration
    {
        public RegisterDto RegisterInfo { get; set; }
        public HospitalProviderCreateDto HospitalProviderInfo { get; set; }
    }
}

