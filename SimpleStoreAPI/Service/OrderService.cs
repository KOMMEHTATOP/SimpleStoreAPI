using Microsoft.EntityFrameworkCore;
using SimpleStoreAPI.Data;
using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.OrderItem;
using SimpleStoreAPI.Interfaces;
using SimpleStoreAPI.Mappers;
using SimpleStoreAPI.Models.Orders;

namespace SimpleStoreAPI.Service;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public OrderService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IEnumerable<OrderResponceDto>>> GetUserOrdersAsync()
    {
        var userId = _currentUserService.GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Result<IEnumerable<OrderResponceDto>>.Failed("No user id");
        }

        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        return Result<IEnumerable<OrderResponceDto>>.Success(orders.Select(OrderMapper.OrderToOrderResponceDto));
    }

    public async Task<Result<OrderResponceDto>> GetByIdAsync(string id)
    {
        var userId = _currentUserService.GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Result<OrderResponceDto>.Failed("No user id");
        }

        var order = await _context.Orders
            .Where(o => o.Id == id && o.UserId == userId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            return Result<OrderResponceDto>.Failed("Order not found");
        }

        return Result<OrderResponceDto>.Success(OrderMapper.OrderToOrderResponceDto(order));
    }

    public async Task<Result<OrderResponceDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var userId = _currentUserService.GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Result<OrderResponceDto>.Failed("User not authorized");
        }

        var productIds = createOrderDto.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
        {
            return Result<OrderResponceDto>.Failed("Some products not found");
        }

        foreach (var item in createOrderDto.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);

            if (product.Stock < item.Quantity)
            {
                return Result<OrderResponceDto>.Failed($"Not enough stock {item.Quantity}");
            }
        }

        var newOrder = new Order()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending,
            TotalAmount = 0
        };

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var item in createOrderDto.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = newOrder.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtPurchase = product.Price
            };

            orderItems.Add(orderItem);
            totalAmount += item.Quantity * product.Price;
        }

        newOrder.TotalAmount = totalAmount;
        newOrder.OrderItems = orderItems;
        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        return Result<OrderResponceDto>.Success(OrderMapper.OrderToOrderResponceDto(newOrder));
    }

    public async Task<Result<OrderResponceDto>> UpdateOrderAsync(string id, UpdateOrderDto updateOrderDto)
    {
        var userId = _currentUserService.GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Result<OrderResponceDto>.Failed("Current user is not authorized");
        }

        var existingOrder = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (existingOrder == null)
        {
            return Result<OrderResponceDto>.Failed("Order not found");
        }

        var isOrderByCurrentUser = existingOrder.UserId == userId;

        if (!isOrderByCurrentUser)
        {
            return Result<OrderResponceDto>.Failed($"Access denied");
        }

        if (existingOrder.Status != OrderStatus.Pending)
        {
            return Result<OrderResponceDto>.Failed("Can only update pending orders");
        }

        _context.OrderItems.RemoveRange(existingOrder.OrderItems);

        var productIds = updateOrderDto.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != productIds.Count)
        {
            return Result<OrderResponceDto>.Failed("Some products not found");
        }

        var newOrderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        foreach (var item in updateOrderDto.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            var newOrderItem = new OrderItem()
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = existingOrder.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtPurchase = product.Price
            };
            newOrderItems.Add(newOrderItem);
            totalAmount += item.Quantity * product.Price;
        }

        existingOrder.TotalAmount = totalAmount;
        existingOrder.OrderItems = newOrderItems;
        _context.Orders.Update(existingOrder);
        await _context.SaveChangesAsync();
        return Result<OrderResponceDto>.Success(OrderMapper.OrderToOrderResponceDto(existingOrder));
    }

    public Task<Result<OrderResponceDto>> UpdateOrderStatusAsync(string id, UpdateOrderStatusDto updateStatusDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CancelOrderAsync(string id)
    {
        throw new NotImplementedException();
    }
}
