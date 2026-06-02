using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;
using FakeDelivery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FakeDelivery.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetByUserIdAsync(int userId)
        => await _context.Orders
            .Where(x => x.UserId == userId)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .ToListAsync();

    public async Task<List<Order>> GetAllAsync()
        => await _context.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .Include(x => x.User)
            .ToListAsync();

    public async Task<Order?> GetByIdAsync(int id)
        => await _context.Orders
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Order> UpdateAsync(Order order)
    {
        order.UpdateDate = DateTime.UtcNow;
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}