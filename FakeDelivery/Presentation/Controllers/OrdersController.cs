using FakeDelivery.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FakeDelivery.Application;

namespace FakeDelivery.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly OrderUseCases _orderUseCases;

    public OrdersController(OrderUseCases orderUseCases)
    {
        _orderUseCases = orderUseCases;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var order = await _orderUseCases.CreateAsync(request, userId);
        return Ok(order);
    }

    [HttpGet("my-orders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var orders = await _orderUseCases.GetMyOrdersAsync(userId);
        return Ok(orders);
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderUseCases.GetAllAsync();
        return Ok(orders);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var order = await _orderUseCases.UpdateStatusAsync(id, request.Status);
        if (order is null) return NotFound();
        return Ok(order);
    }
}