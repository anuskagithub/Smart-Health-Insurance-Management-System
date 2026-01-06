namespace HealthInsuranceApi.DTOs.HospitalProvider
{
    public class HospitalProviderUpdateDto
    {
        public string ProviderName { get; set; }

        public string ProviderType { get; set; }
        // Hospital / Clinic / Lab / NursingHome

        public string City { get; set; }

        public bool IsNetworkProvider { get; set; }

        public bool IsActive { get; set; }
    }
}
