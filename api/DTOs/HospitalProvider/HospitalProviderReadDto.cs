namespace HealthInsuranceApi.DTOs.HospitalProvider
{
    public class HospitalProviderReadDto
    {
        public int HospitalProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderType { get; set; }
        public string City { get; set; }
        public bool IsNetworkProvider { get; set; }
        public bool IsActive { get; set; }
    }
}
