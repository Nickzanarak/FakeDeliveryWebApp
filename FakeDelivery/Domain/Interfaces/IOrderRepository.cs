using FakeDelivery.Domain.Entities;

namespace FakeDelivery.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<List<Order>> GetByUserIdAsync(int userId);
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order> UpdateAsync(Order order);
}