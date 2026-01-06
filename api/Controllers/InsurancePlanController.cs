using HealthInsuranceApi.DTOs.InsurancePlan;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class InsurancePlanController : ControllerBase
{
    private readonly IInsurancePlanService _service;

    public InsurancePlanController(IInsurancePlanService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(InsurancePlanCreateDto dto)
    {
        await _service.CreatePlanAsync(dto);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, InsurancePlanUpdateDto dto)
    {
        await _service.UpdatePlanAsync(id, dto);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllPlansAsync());

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/disable")]
    public async Task<IActionResult> Disable(int id)
    {
        await _service.DisablePlanAsync(id);
        return Ok();
    }
}
