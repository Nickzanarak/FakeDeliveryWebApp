using FakeDelivery.Domain.Entities;

namespace FakeDelivery.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(int userId);
    Task<Cart> CreateAsync(int userId);
    Task<CartItem> AddItemAsync(int cartId, int productId, int quantity);
    Task UpdateItemAsync(CartItem item);
    Task RemoveItemAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
}