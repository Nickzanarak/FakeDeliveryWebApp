using FakeDelivery.Application;
using FakeDelivery.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeDelivery.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class FilesController : ControllerBase
{
    private readonly IFileRepository _fileRepo;

    public FilesController(IFileRepository fileRepo)
    {
        _fileRepo = fileRepo;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var userId = int.Parse(User.FindFirst("id")!.Value);
        var fileId = await _fileRepo.SaveFileAsync(file, userId);
        if (fileId is null) return BadRequest(new { message = "ไม่พบไฟล์" });
        return Ok(new FileResponse { Id = fileId.Value });
    }
}