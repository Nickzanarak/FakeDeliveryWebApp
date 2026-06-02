namespace FakeDelivery.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
    public DateTime? CreateDate { get; set; }
    public int? CreateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? UpdateBy { get; set; }
    public bool? IsDelete { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}