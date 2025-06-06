using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleStoreAPI.Models;
using SimpleStoreAPI.Models.Orders;

namespace SimpleStoreAPI.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> products { get; set; }
        DbSet<Order> orders { get; set; }
        DbSet<OrderItem> orderItem { get; set; }
    }
}
