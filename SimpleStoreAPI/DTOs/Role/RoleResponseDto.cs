using System.Runtime.InteropServices.JavaScript;

namespace SimpleStoreAPI.DTOs.Role;

public class RoleResponseDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
