using FakeDelivery.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FakeDelivery.Application;

namespace FakeDelivery.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class UsersController : ControllerBase
{
    private readonly UserUseCases _userUseCases;

    public UsersController(UserUseCases userUseCases)
    {
        _userUseCases = userUseCases;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userUseCases.GetAllAsync();
        return Ok(users);
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
    {
        var user = await _userUseCases.UpdateRoleAsync(id, request.Role);
        if (user is null) return BadRequest(new { message = "Role ไม่ถูกต้อง หรือไม่พบ User" });
        return Ok(user);
    }
}