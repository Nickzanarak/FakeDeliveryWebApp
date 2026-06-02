namespace FakeDelivery.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = "shopping";
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public User User { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}