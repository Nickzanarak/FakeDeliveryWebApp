using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;
using FakeDelivery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FakeDelivery.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
        => await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<User?> GetByUsernameAsync(string username)
        => await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<List<User>> GetAllAsync()
        => await _context.Users
            .Where(x => x.IsDelete != true)
            .ToListAsync();

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}