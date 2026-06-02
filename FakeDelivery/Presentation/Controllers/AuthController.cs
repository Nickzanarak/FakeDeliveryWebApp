using FakeDelivery.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using FakeDelivery.Application;

namespace FakeDelivery.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthUseCases _authUseCases;

    public AuthController(AuthUseCases authUseCases)
    {
        _authUseCases = authUseCases;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authUseCases.RegisterAsync(request);
        if (result is null) return BadRequest(new { message = "Username นี้มีอยู่แล้ว" });
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authUseCases.LoginAsync(request);
        if (result is null) return Unauthorized(new { message = "Username หรือ Password ไม่ถูกต้อง" });
        return Ok(result);
    }
}