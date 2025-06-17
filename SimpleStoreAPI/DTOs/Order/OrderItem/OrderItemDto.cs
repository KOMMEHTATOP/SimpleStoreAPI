namespace SimpleStoreAPI.DTOs.OrderItem;

public class OrderItemDto
{
    public required string ProductId { get; set; }
    public int Quantity { get; set; }
}
