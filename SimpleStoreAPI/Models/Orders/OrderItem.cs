namespace SimpleStoreAPI.Models.Orders
{
    public class OrderItem
    {
        public string Id { get; set; }
        public string OrderId { get; set; } // Внешний ключ для заказа
        public string ProductId { get; set; } // Внешний ключ для продукта
        public int Quantity { get; set; } // Количество товара в заказе
        public decimal PriceAtPurchase { get; set; } // Цена товара на момент заказа

        // Связи с другими сущностями
        public Order Order { get; set; } = null!; // Навигационное свойство для заказа
        public Product Product { get; set; } = null!; // Навигационное свойство для продукта
    }
}