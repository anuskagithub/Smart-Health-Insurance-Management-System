using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.InsurancePlan;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class InsurancePlanService : IInsurancePlanService
{
    private readonly ApplicationDbContext _context;

    public InsurancePlanService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreatePlanAsync(InsurancePlanCreateDto dto)
    {
        var plan = new InsurancePlan
        {
            PlanName = dto.PlanName,
            PlanType = dto.PlanType,
            PremiumAmount = dto.PremiumAmount,
            CoverageAmount = dto.CoverageAmount,
            DurationInMonths = dto.DurationInMonths,
            IsActive = true
        };

        _context.InsurancePlans.Add(plan);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePlanAsync(int planId, InsurancePlanUpdateDto dto)
    {
        var plan = await _context.InsurancePlans.FindAsync(planId)
            ?? throw new Exception("Plan not found");

        plan.PlanName = dto.PlanName;
        plan.PremiumAmount = dto.PremiumAmount;
        plan.PlanType = dto.PlanType;
        plan.CoverageAmount = dto.CoverageAmount;
        plan.DurationInMonths = dto.DurationInMonths;

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<InsurancePlanReadDto>> GetAllPlansAsync()
    {
        return await _context.InsurancePlans
            .Select(p => new InsurancePlanReadDto
            {
                InsurancePlanId = p.InsurancePlanId,
                PlanName = p.PlanName,
                PlanType = p.PlanType,
                PremiumAmount = p.PremiumAmount,
                CoverageAmount = p.CoverageAmount,
                DurationInMonths = p.DurationInMonths,
                IsActive = p.IsActive
            })
            .ToListAsync();
    }

    public async Task DisablePlanAsync(int planId)
    {
        var plan = await _context.InsurancePlans.FindAsync(planId)
            ?? throw new Exception("Plan not found");

        plan.IsActive = false;
        await _context.SaveChangesAsync();
    }

    public async Task TogglePlanStatusAsync(int planId, bool isActive)
    {
        var plan = await _context.InsurancePlans.FindAsync(planId)
            ?? throw new Exception("Plan not found");

        plan.IsActive = isActive;
        await _context.SaveChangesAsync();
    }
}
