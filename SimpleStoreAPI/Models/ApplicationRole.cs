using Microsoft.AspNetCore.Identity;

namespace SimpleStoreAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
