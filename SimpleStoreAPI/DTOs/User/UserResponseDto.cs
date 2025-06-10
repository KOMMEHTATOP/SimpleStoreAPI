namespace SimpleStoreAPI.DTOs.User;

public class UserResponseDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = null!;
}
