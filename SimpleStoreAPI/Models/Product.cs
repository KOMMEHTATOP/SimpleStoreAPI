namespace SimpleStoreAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public required string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //Связи с другими сущностями

        public string SellerId { get; set; } = null!; //Внешний ключ (Foreign Keys)
        public ApplicationUser Seller { get; set; } = null!; //Навигационное свойство (Navigation Properties)
    }
}
