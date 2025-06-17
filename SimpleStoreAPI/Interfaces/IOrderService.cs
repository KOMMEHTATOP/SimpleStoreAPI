using SimpleStoreAPI.DTOs;

namespace SimpleStoreAPI.Interfaces;

public interface IOrderService
{
    Task<Result<IEnumerable<OrderResponceDto>>> GetUserOrdersAsync(); 
    Task<Result<OrderResponceDto>> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<Result<OrderResponceDto>> GetByIdAsync(string id);
    Task<Result<OrderResponceDto>> UpdateOrderAsync(string id, UpdateOrderDto updateOrderDto);
    Task<Result<OrderResponceDto>> UpdateOrderStatusAsync(string id, UpdateOrderStatusDto updateStatusDto);
    Task<Result> CancelOrderAsync(string id);
}
