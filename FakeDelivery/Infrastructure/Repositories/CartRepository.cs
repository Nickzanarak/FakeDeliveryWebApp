using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;
using FakeDelivery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FakeDelivery.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(int userId)
        => await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.File)
            .FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task<Cart> CreateAsync(int userId)
    {
        var cart = new Cart
        {
            UserId = userId,
            CreateDate = DateTime.UtcNow
        };
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<CartItem> AddItemAsync(int cartId, int productId, int quantity)
    {
        var item = new CartItem
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            CreateDate = DateTime.UtcNow
        };
        _context.CartItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task UpdateItemAsync(CartItem item)
    {
        item.UpdateDate = DateTime.UtcNow;
        _context.CartItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveItemAsync(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        if (item is not null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(int cartId)
    {
        var items = await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();
        _context.CartItems.RemoveRange(items);
        await _context.SaveChangesAsync();
    }
}