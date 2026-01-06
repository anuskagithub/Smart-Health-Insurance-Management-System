namespace HealthInsuranceApi.DTOs.Payment
{
    public class PaymentCreateDto
    {
        public int PolicyId { get; set; }          // 0 = new purchase
        public int InsurancePlanId { get; set; }   // REQUIRED for new purchase
        public decimal Amount { get; set; }
        public bool IsGatewayConfirmed { get; set; }
    }

}

