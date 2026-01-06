namespace HealthInsuranceApi.DTOs.Payment
{
    public class PaymentReadDto
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
