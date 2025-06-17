using SimpleStoreAPI.DTOs.OrderItem;

namespace SimpleStoreAPI.DTOs;

public class CreateOrderDto
{
    public required List<OrderItemDto> Items { get; set; }
}
