using FakeDelivery.Domain.Interfaces;

namespace FakeDelivery.Application.UseCases;

public class UserUseCases
{
    private readonly IUserRepository _userRepo;

    public UserUseCases(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = await _userRepo.GetAllAsync();
        return users.Select(u => new UserResponse
        {
            Id = u.Id,
            Username = u.Username,
            Name = u.Name,
            Role = u.Role
        }).ToList();
    }

    public async Task<UserResponse?> UpdateRoleAsync(int id, string role)
    {
        if (role != "admin" && role != "user") return null;

        var user = await _userRepo.GetByIdAsync(id);
        if (user is null) return null;

        user.Role = role;
        user.UpdateDate = DateTime.UtcNow;
        var updated = await _userRepo.UpdateAsync(user);

        return new UserResponse
        {
            Id = updated.Id,
            Username = updated.Username,
            Name = updated.Name,
            Role = updated.Role
        };
    }
}