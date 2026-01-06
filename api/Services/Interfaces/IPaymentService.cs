using HealthInsuranceApi.DTOs.Payment;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<int> PayPremiumAsync(PaymentCreateDto dto, string userId);

        Task MakeClaimPayoutAsync(int claimId);

        Task<IEnumerable<PaymentReadDto>>
            GetPaymentsByPolicyAsync(int policyId);
    }
}

