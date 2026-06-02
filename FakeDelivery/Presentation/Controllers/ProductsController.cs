using FakeDelivery.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FakeDelivery.Application;

namespace FakeDelivery.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductUseCases _productUseCases;

    public ProductsController(ProductUseCases productUseCases)
    {
        _productUseCases = productUseCases;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productUseCases.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productUseCases.GetByIdAsync(id);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var product = await _productUseCases.CreateAsync(request, userId);
        return Ok(product);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var product = await _productUseCases.UpdateAsync(id, request, userId);
        if (product is null) return NotFound();
        return Ok(product);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productUseCases.DeleteAsync(id);
        return Ok(new { message = "ลบสินค้าเรียบร้อย" });
    }
}