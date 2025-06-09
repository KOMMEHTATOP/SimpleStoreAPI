using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SimpleStoreAPI.Models
{
    public class ApplicationRole : IdentityRole
    {
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
