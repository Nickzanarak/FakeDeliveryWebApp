using FakeDelivery.Domain.Entities;

namespace FakeDelivery.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(User user);
    Task<List<User>> GetAllAsync();
    Task<User> UpdateAsync(User user);
}