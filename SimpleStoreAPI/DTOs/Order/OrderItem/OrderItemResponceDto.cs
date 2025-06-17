namespace SimpleStoreAPI.DTOs.OrderItem;

public class OrderItemResponceDto
{
    public required string ProductName { get; set; } 
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
    public decimal TotalPrice { get; set; }
}
