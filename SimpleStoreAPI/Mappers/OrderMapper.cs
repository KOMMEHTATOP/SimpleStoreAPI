using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.OrderItem;
using SimpleStoreAPI.Models.Orders;

namespace SimpleStoreAPI.Mappers;

public class OrderMapper
{
    public static OrderResponceDto OrderToOrderResponceDto(Order order)
    {
        return new OrderResponceDto()
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi=> OrderItemToOrderItemResponceDto(oi)).ToList()
        };
    }
    
    private static OrderItemResponceDto OrderItemToOrderItemResponceDto(OrderItem orderItem)
    {
        return new OrderItemResponceDto()
        {
            ProductName = orderItem.Product.Name,
            Quantity = orderItem.Quantity,
            PriceAtPurchase = orderItem.PriceAtPurchase,
            TotalPrice = orderItem.PriceAtPurchase * orderItem.Quantity
        };
    }
}
