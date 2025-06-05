namespace SimpleStoreAPI.Models.Orders
{
    public enum OrderStatus
    {
        Pending,    // Заказ ожидает обработки
        Completed,  // Заказ выполнен
        Cancelled,  // Заказ отменен
        Shipped,    // Заказ отправлен
        Delivered,  // Заказ доставлен
        Returned    // Заказ возвращен
    }
}
