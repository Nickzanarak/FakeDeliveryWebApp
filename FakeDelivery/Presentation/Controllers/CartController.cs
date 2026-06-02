using FakeDelivery.Application;
using FakeDelivery.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeDelivery.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly CartUseCases _cartUseCases;

    public CartController(CartUseCases cartUseCases)
    {
        _cartUseCases = cartUseCases;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var cart = await _cartUseCases.GetCartAsync(userId);
        return Ok(cart);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var cart = await _cartUseCases.AddToCartAsync(userId, request);
        return Ok(cart);
    }

    [HttpPut("items/{cartItemId}")]
    public async Task<IActionResult> UpdateItem(int cartItemId, [FromBody] UpdateCartItemRequest request)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var cart = await _cartUseCases.UpdateItemAsync(userId, cartItemId, request.Quantity);
        return Ok(cart);
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var cart = await _cartUseCases.RemoveItemAsync(userId, cartItemId);
        return Ok(cart);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        await _cartUseCases.ClearCartAsync(userId);
        return Ok(new { message = "ล้างตะกร้าเรียบร้อย" });
    }
}