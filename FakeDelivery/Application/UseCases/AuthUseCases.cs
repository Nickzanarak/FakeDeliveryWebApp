using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FakeDelivery.Application.UseCases;

public class AuthUseCases
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthUseCases(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var existing = await _userRepo.GetByUsernameAsync(request.Username);
        if (existing is not null) return null;

        var user = new User
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Name = request.Name,
            Role = request.Role,
            CreateDate = DateTime.UtcNow,
            IsDelete = false
        };

        await _userRepo.CreateAsync(user);
        return MapToResponse(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepo.GetByUsernameAsync(request.Username);
        if (user is null) return null;
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) return null;

        return MapToResponse(user);
    }

    private AuthResponse MapToResponse(User user)
    {
        return new AuthResponse
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Role = user.Role,
            Token = GenerateToken(user)
        };
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("name", user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}