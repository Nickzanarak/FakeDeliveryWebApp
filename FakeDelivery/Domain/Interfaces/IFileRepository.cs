using Microsoft.AspNetCore.Http;

namespace FakeDelivery.Domain.Interfaces;

public interface IFileRepository
{
    Task<int?> SaveFileAsync(IFormFile uploadFile, int userId);
}