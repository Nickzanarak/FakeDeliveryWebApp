namespace FakeDelivery.Application;

// ==================== Auth ====================
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
}

public class AuthResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

// ==================== Product ====================
public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? FileId { get; set; }
}

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int? FileId { get; set; }
}

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? FileId { get; set; }
}

// ==================== Order ====================
public class CreateOrderRequest
{
    public List<OrderItemRequest> Items { get; set; } = new();
}

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class OrderResponse
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}

public class OrderItemResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}

public class UpdateStatusRequest
{
    public string Status { get; set; } = string.Empty;
}

// ==================== File ====================
public class FileResponse
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
}

// ==================== User ====================
public class UserResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

// ==================== Cart ====================
public class AddToCartRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateCartItemRequest
{
    public int Quantity { get; set; }
}

public class CartResponse
{
    public int Id { get; set; }
    public List<CartItemResponse> Items { get; set; } = new();
    public decimal TotalPrice { get; set; }
}

public class CartItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
public class UpdateRoleRequest
{
    public string Role { get; set; } = string.Empty;
}