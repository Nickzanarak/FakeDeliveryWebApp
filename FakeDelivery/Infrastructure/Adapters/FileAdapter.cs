using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;
using FakeDelivery.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;

namespace FakeDelivery.Infrastructure.Adapters;

public class FileAdapter : IFileRepository
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public FileAdapter(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<int?> SaveFileAsync(IFormFile uploadFile, int userId)
    {
        if (uploadFile == null || uploadFile.Length == 0) return null;

        string extension = Path.GetExtension(uploadFile.FileName).ToLower();
        string fileName = Guid.NewGuid().ToString() + extension;

        string folderPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await uploadFile.CopyToAsync(stream);
        }

        var fileEntity = new FileEntity
        {
            FileName = uploadFile.FileName,
            FilePath = "/uploads/" + fileName,
            FileSize = (int)uploadFile.Length,
            FileType = uploadFile.ContentType,
            CreateDate = DateTime.UtcNow,
            CreateBy = userId,
            IsDelete = false
        };

        _context.Files.Add(fileEntity);
        await _context.SaveChangesAsync();

        return fileEntity.Id;
    }
}