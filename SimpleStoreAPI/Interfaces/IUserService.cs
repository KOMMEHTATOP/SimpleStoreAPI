using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.User;

namespace SimpleStoreAPI.Interfaces;

public interface IUserService
{
    Task<Result<UserResponseDto>> GetByIdAsync(string id);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<Result<UserResponseDto>> UpdateAsync(string id, UpdateUserDto userDto);
    Task<Result> DeleteAsync(string id);
}
