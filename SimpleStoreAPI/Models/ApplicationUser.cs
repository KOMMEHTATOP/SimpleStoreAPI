using Microsoft.AspNetCore.Identity;

namespace SimpleStoreAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>(); //навигационные свойства (чтобы доставать где нужно)
        public bool IsDeleted { get; set; } = false; // Флаг для логического удаления пользователя
        public DateTime? DeletedAt { get; set; } // Дата и время логического удаления пользователя
    }   
}
