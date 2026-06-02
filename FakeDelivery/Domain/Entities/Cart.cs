namespace FakeDelivery.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public User User { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }

    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;
}