using SimpleStoreAPI.DTOs.User;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Mappers;

public class UserMapper
{
    public static UserResponseDto ApplicationUserToDto(ApplicationUser user, IEnumerable<string> roles)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            Roles = roles.ToList()
        };
    }
}
