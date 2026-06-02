using FakeDelivery.Domain.Entities;
using FakeDelivery.Domain.Interfaces;

namespace FakeDelivery.Application.UseCases;

public class OrderUseCases
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public OrderUseCases(IOrderRepository orderRepo, IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<OrderResponse?> CreateAsync(CreateOrderRequest request, int userId)
    {
        var order = new Order
        {
            UserId = userId,
            Status = "shopping",
            CreateDate = DateTime.UtcNow,
            OrderItems = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                CreateDate = DateTime.UtcNow
            }).ToList()
        };

        var created = await _orderRepo.CreateAsync(order);
        return await MapToResponseAsync(created);
    }

    public async Task<List<OrderResponse>> GetMyOrdersAsync(int userId)
    {
        var orders = await _orderRepo.GetByUserIdAsync(userId);
        var responses = new List<OrderResponse>();
        foreach (var order in orders)
            responses.Add(await MapToResponseAsync(order));
        return responses;
    }

    public async Task<List<OrderResponse>> GetAllAsync()
    {
        var orders = await _orderRepo.GetAllAsync();
        var responses = new List<OrderResponse>();
        foreach (var order in orders)
            responses.Add(await MapToResponseAsync(order));
        return responses;
    }

    public async Task<OrderResponse?> UpdateStatusAsync(int orderId, string status)
    {
        var order = await _orderRepo.GetByIdAsync(orderId);
        if (order is null) return null;

        order.Status = status;
        var updated = await _orderRepo.UpdateAsync(order);
        return await MapToResponseAsync(updated);
    }

    private async Task<OrderResponse> MapToResponseAsync(Order order)
    {
        var items = new List<OrderItemResponse>();
        foreach (var item in order.OrderItems)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            items.Add(new OrderItemResponse
            {
                ProductId = item.ProductId,
                ProductName = product?.Name ?? "",
                Price = product?.Price ?? 0,
                Quantity = item.Quantity,
                Subtotal = (product?.Price ?? 0) * item.Quantity
            });
        }

        return new OrderResponse
        {
            Id = order.Id,
            Status = order.Status,
            CreateDate = order.CreateDate ?? DateTime.UtcNow,
            Items = items,
            TotalPrice = items.Sum(i => i.Subtotal)
        };
    }
}