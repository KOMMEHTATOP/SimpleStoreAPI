namespace SimpleStoreAPI.DTOs.Role
{
    public class CreateRoleDto
    {
        public string Name{ get; set; } = null!;
        public string? Description { get; set; }
    }
}
