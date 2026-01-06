using HealthInsuranceApi.Data;
using HealthInsuranceApi.DTOs.Policy;
using HealthInsuranceApi.Models;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PolicyService : IPolicyService
{
    private readonly ApplicationDbContext _context;

    public PolicyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> EnrollPolicyAsync(PolicyCreateDto dto)
    {
        var plan = await _context.InsurancePlans.FindAsync(dto.InsurancePlanId)
            ?? throw new Exception("Plan not found");

        if (!plan.IsActive)
            throw new Exception("Plan is inactive");

        var start = DateTime.UtcNow;
        var end = start.AddMonths(plan.DurationInMonths);

        var policy = new Policy
        {
            CustomerProfileId = dto.CustomerProfileId,
            InsurancePlanId = dto.InsurancePlanId,
            PolicyNumber = Guid.NewGuid().ToString("N")[..10],

            StartDate = start,
            EndDate = end,
            Status = "Active",

            //Premium calculation
            PremiumAmount = plan.PremiumAmount,

            //Coverage initialization
            CoverageRemaining = plan.CoverageAmount

            


        }; 

        _context.Policies.Add(policy);
        await _context.SaveChangesAsync();

        return policy.PolicyId;
    }

    public async Task RenewPolicyAsync(int policyId)
    {
        var policy = await _context.Policies
            .Include(p => p.InsurancePlan)
            .FirstOrDefaultAsync(p => p.PolicyId == policyId)
            ?? throw new Exception("Policy not found");

        policy.StartDate = DateTime.UtcNow;
        policy.EndDate = policy.StartDate
            .AddMonths(policy.InsurancePlan.DurationInMonths);

        policy.Status = "Active";

        //RESET coverage on renewal
        policy.CoverageRemaining = policy.InsurancePlan.CoverageAmount;

        await _context.SaveChangesAsync();
    }

   

    public async Task SuspendPolicyAsync(int policyId)
    {
        var policy = await _context.Policies.FindAsync(policyId)
            ?? throw new Exception("Policy not found");

        policy.Status = "Suspended";
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PolicyReadDto>> GetPoliciesAsync()
    {
        return await _context.Policies
            .Include(p => p.InsurancePlan)
            .Select(p => new PolicyReadDto
            {
                PolicyId = p.PolicyId,
                PolicyNumber = p.PolicyNumber,
                PlanName = p.InsurancePlan.PlanName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                CoverageRemaining = p.CoverageRemaining,
                PremiumAmount = p.PremiumAmount
            })
            .ToListAsync();
    }

    public async Task<PolicyReadDto?> GetPolicyByIdAsync(int policyId)
    {
        return await _context.Policies
    .Include(p => p.InsurancePlan)
    .Where(p => p.PolicyId == policyId)
    .Select(p => new PolicyReadDto
    {
        PolicyId = p.PolicyId,
        PolicyNumber = p.PolicyNumber,
        PlanName = p.InsurancePlan.PlanName,
        StartDate = p.StartDate,
        EndDate = p.EndDate,
        Status = p.Status,
        CoverageRemaining=p.CoverageRemaining,
        PremiumAmount=p.PremiumAmount
    })
    .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PolicyTypeStatusReportDto>> GetPolicyTypeStatusReportAsync()
    {
        return await _context.Policies
    .Include(p => p.InsurancePlan)
    .GroupBy(p => p.InsurancePlan.PlanName)
    .Select(g => new PolicyTypeStatusReportDto
    {
        PlanName = g.Key,

        PolicyStatusCount = g
            .GroupBy(x => x.Status)
            .Select(s => new PolicyStatusCount
            {
                Status = s.Key,
                Count = s.Count()
            })
            .ToList()
    })
    .ToListAsync();
    }
}
