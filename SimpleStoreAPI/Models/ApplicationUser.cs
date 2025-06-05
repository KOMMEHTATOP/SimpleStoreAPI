using Microsoft.AspNetCore.Identity;

namespace SimpleStoreAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; } = false; // Флаг для логического удаления пользователя
        public DateTime? DeletedAt { get; set; } // Дата и время логического удаления пользователя
    }   
}
