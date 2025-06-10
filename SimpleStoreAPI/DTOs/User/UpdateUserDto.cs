namespace SimpleStoreAPI.DTOs.User;

public class UpdateUserDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = null!;
}
