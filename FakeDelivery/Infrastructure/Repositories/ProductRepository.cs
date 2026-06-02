using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;
using FakeDelivery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FakeDelivery.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _context.Products
            .Where(x => x.IsDelete != true)
            .Include(x => x.File)
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _context.Products
            .Where(x => x.IsDelete != true)
            .Include(x => x.File)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

         await _context.Entry(product)
        .Reference(p => p.File)
        .LoadAsync();
        
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        product.UpdateDate = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        await _context.Entry(product)
        .Reference(p => p.File)
        .LoadAsync();
        
        return product;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product is null) return;

        var cartItems = await _context.CartItems
            .Where(ci => ci.ProductId == id)
            .ToListAsync();
        _context.CartItems.RemoveRange(cartItems);

        product.IsDelete = true;
        product.UpdateDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}