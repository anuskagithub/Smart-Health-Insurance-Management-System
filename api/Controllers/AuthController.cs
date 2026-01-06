using HealthInsuranceApi.DTOs.Auth;
using HealthInsuranceApi.Services.Implementations;
using HealthInsuranceApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HealthInsuranceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICustomerService _customerService;

        public AuthController(IAuthService authService, ICustomerService customerService)
        {

            _authService = authService;
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerRegistration custregist)
        {
            var userId = await _authService.RegisterAsync(custregist.RegisterInfo);
            await _customerService.CreateCustomerProfileAsync(custregist.CustProfileInfo, userId);

            return Ok(new
            {
                message = "Registration successful. Await admin approval.",
                userId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }
    }
}
