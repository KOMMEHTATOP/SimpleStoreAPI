using SimpleStoreAPI.DTOs.OrderItem;
using SimpleStoreAPI.Models.Orders;

namespace SimpleStoreAPI.DTOs;

public class OrderResponceDto
{
    public string Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemResponceDto> Items { get; set; }
}
