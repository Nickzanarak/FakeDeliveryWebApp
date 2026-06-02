namespace FakeDelivery.Domain.Entities;

public class FileEntity
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public int FileSize { get; set; }
    public string? FileType { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? CreateBy { get; set; }
    public bool? IsDelete { get; set; }
}