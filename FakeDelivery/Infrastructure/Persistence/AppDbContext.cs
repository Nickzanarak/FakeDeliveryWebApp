using FakeDelivery.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FakeDelivery.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<FileEntity> Files => Set<FileEntity>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Username).HasColumnName("username");
            e.Property(x => x.Password).HasColumnName("password");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Role).HasColumnName("role");
            e.HasMany(x => x.Orders).WithOne(o => o.User).HasForeignKey(o => o.UserId);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("products");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.Price).HasColumnName("price");
            e.Property(x => x.FileId).HasColumnName("file_id");
            e.HasOne(x => x.File).WithMany().HasForeignKey(x => x.FileId);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("orders");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.Status).HasColumnName("status");
            e.HasMany(x => x.OrderItems).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId);
        });

        modelBuilder.Entity<OrderItem>(e =>
        {
            e.ToTable("order_items");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.OrderId).HasColumnName("order_id");
            e.Property(x => x.ProductId).HasColumnName("product_id");
            e.Property(x => x.Quantity).HasColumnName("quantity");
            e.HasOne(x => x.Product).WithMany(p => p.OrderItems).HasForeignKey(x => x.ProductId);
        });

        modelBuilder.Entity<FileEntity>(e =>
        {
            e.ToTable("files");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.FileName).HasColumnName("file_name");
            e.Property(x => x.FilePath).HasColumnName("file_path");
            e.Property(x => x.FileSize).HasColumnName("file_size");
            e.Property(x => x.FileType).HasColumnName("file_type");
        });

        modelBuilder.Entity<Cart>(e =>
        {
            e.ToTable("carts");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            e.HasMany(x => x.CartItems).WithOne(ci => ci.Cart).HasForeignKey(ci => ci.CartId);
        });

        modelBuilder.Entity<CartItem>(e =>
        {
            e.ToTable("cart_items");
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.CartId).HasColumnName("cart_id");
            e.Property(x => x.ProductId).HasColumnName("product_id");
            e.Property(x => x.Quantity).HasColumnName("quantity");
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        });
    }
}