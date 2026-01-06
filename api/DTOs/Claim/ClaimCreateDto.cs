namespace HealthInsuranceApi.DTOs.Claim
{
    public class ClaimCreateDto
    {
        public int PolicyId { get; set; }
        public int HospitalProviderId { get; set; }
        public decimal ClaimAmount { get; set; }
        public string Remarks { get; set; }
    }
}
