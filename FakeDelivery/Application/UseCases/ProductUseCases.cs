using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;

namespace FakeDelivery.Application.UseCases;

public class ProductUseCases
{
    private readonly IProductRepository _productRepo;

    public ProductUseCases(IProductRepository productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<List<ProductResponse>> GetAllAsync()
    {
        var products = await _productRepo.GetAllAsync();
        return products.Select(MapToResponse).ToList();
    }

    public async Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, int createdBy)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            FileId = request.FileId,
            CreateDate = DateTime.UtcNow,
            CreateBy = createdBy,
            IsDelete = false
        };

        var created = await _productRepo.CreateAsync(product);
        return MapToResponse(created);
    }

    public async Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request, int updatedBy)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product is null) return null;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.FileId = request.FileId;
        product.UpdateDate = DateTime.UtcNow;
        product.UpdateBy = updatedBy;

        var updated = await _productRepo.UpdateAsync(product);
        return MapToResponse(updated);
    }

    public async Task DeleteAsync(int id)
        => await _productRepo.DeleteAsync(id);

    private ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.File?.FilePath,
            FileId = product.FileId
        };
    }
}