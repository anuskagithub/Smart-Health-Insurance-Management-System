namespace HealthInsuranceApi.DTOs.HospitalProvider
{
    public class HospitalProviderCreateDto
    {
        public string? UserId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderType { get; set; }
        public string City { get; set; }
        public bool IsNetworkProvider { get; set; }
    }
}
