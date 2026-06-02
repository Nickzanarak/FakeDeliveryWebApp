namespace FakeDelivery.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? FileId { get; set; }
    public DateTime? CreateDate { get; set; }
    public int? CreateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? UpdateBy { get; set; }
    public bool? IsDelete { get; set; }

    public FileEntity? File { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}