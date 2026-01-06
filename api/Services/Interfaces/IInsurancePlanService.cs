using HealthInsuranceApi.DTOs.InsurancePlan;

namespace HealthInsuranceApi.Services.Interfaces
{
    public interface IInsurancePlanService
    {
        Task CreatePlanAsync(InsurancePlanCreateDto dto);
        Task TogglePlanStatusAsync(int planId, bool isActive);
        Task UpdatePlanAsync(int planId, InsurancePlanUpdateDto dto);
        Task<IEnumerable<InsurancePlanReadDto>> GetAllPlansAsync();
        Task DisablePlanAsync(int planId);
    }
}

