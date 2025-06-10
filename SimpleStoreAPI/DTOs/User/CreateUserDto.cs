namespace SimpleStoreAPI.DTOs.User;

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Password { get; set; } = null!;
    public List<string> Roles { get; set; } = null!;
}
