using SimpleStoreAPI.Models.Orders;

namespace SimpleStoreAPI.DTOs;

public class UpdateOrderStatusDto
{
    public required OrderStatus Status { get; set; }
}
