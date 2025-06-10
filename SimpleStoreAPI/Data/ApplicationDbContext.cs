using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleStoreAPI.Models;
using SimpleStoreAPI.Models.Orders;
using System.Reflection.Emit;

namespace SimpleStoreAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Отключить каскадное удаление для Products
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)  // Указываем навигационное свойство
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //Настройка для навигационных свойств (чтобы можно было доставать роли одним запросом вместе с юзером
            //Так же к этим настройкам применил навигационные свойства в классах ApplicationUser/Role все нижние 3 блока
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserRoles)
                .WithOne()
                .HasForeignKey("UserId");
            
            modelBuilder.Entity<ApplicationRole>()
                .HasMany(r=>r.UserRoles)
                .WithOne()
                .HasForeignKey("RoleId");
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
    }
}
