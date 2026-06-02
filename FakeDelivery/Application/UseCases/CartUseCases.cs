using FakeDelivery.Domain.Interfaces;

namespace FakeDelivery.Application.UseCases;

public class CartUseCases
{
    private readonly ICartRepository _cartRepo;

    public CartUseCases(ICartRepository cartRepo)
    {
        _cartRepo = cartRepo;
    }

    public async Task<CartResponse> GetCartAsync(int userId)
    {
        var cart = await _cartRepo.GetByUserIdAsync(userId);
        if (cart is null) return new CartResponse { Items = new(), TotalPrice = 0 };
        return MapToResponse(cart);
    }

    public async Task<CartResponse> AddToCartAsync(int userId, AddToCartRequest request)
    {
        var cart = await _cartRepo.GetByUserIdAsync(userId);
        if (cart is null) cart = await _cartRepo.CreateAsync(userId);

        var existing = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
        if (existing is not null)
        {
            existing.Quantity += request.Quantity;
            await _cartRepo.UpdateItemAsync(existing);
        }
        else
        {
            await _cartRepo.AddItemAsync(cart.Id, request.ProductId, request.Quantity);
        }

        var updated = await _cartRepo.GetByUserIdAsync(userId);
        return MapToResponse(updated!);
    }

    public async Task<CartResponse> UpdateItemAsync(int userId, int cartItemId, int quantity)
    {
        var cart = await _cartRepo.GetByUserIdAsync(userId);
        if (cart is null) return new CartResponse();

        var item = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (item is null) return MapToResponse(cart);

        if (quantity <= 0)
        {
            await _cartRepo.RemoveItemAsync(cartItemId);
        }
        else
        {
            item.Quantity = quantity;
            await _cartRepo.UpdateItemAsync(item);
        }

        var updated = await _cartRepo.GetByUserIdAsync(userId);
        return MapToResponse(updated!);
    }

    public async Task<CartResponse> RemoveItemAsync(int userId, int cartItemId)
    {
        await _cartRepo.RemoveItemAsync(cartItemId);
        var updated = await _cartRepo.GetByUserIdAsync(userId);
        return updated is null ? new CartResponse() : MapToResponse(updated);
    }

    public async Task ClearCartAsync(int userId)
    {
        var cart = await _cartRepo.GetByUserIdAsync(userId);
        if (cart is not null) await _cartRepo.ClearCartAsync(cart.Id);
    }

    private CartResponse MapToResponse(Domain.Entities.Cart cart)
    {
        var items = cart.CartItems.Select(ci => new CartItemResponse
        {
            Id = ci.Id,
            ProductId = ci.ProductId,
            ProductName = ci.Product?.Name ?? "",
            Price = ci.Product?.Price ?? 0,
            ImageUrl = ci.Product?.File?.FilePath,
            Quantity = ci.Quantity,
            Subtotal = (ci.Product?.Price ?? 0) * ci.Quantity
        }).ToList();

        return new CartResponse
        {
            Id = cart.Id,
            Items = items,
            TotalPrice = items.Sum(i => i.Subtotal)
        };
    }
}