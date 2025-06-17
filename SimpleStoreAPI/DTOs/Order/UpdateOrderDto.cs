using SimpleStoreAPI.DTOs.OrderItem;

namespace SimpleStoreAPI.DTOs;

public class UpdateOrderDto
{
    public required List<OrderItemDto> Items { get; set; }
}
