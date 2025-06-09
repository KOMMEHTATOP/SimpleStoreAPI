namespace SimpleStoreAPI.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public required string UserId { get; set; } // Внешний ключ для пользователя
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } // Статус заказа (Pending, Completed, Cancelled и т.д.)
        
        // Связи с другими сущностями
        public ApplicationRole User { get; set; } = null!; // Навигационное свойство для пользователя
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Список позиций заказа
    }
}
