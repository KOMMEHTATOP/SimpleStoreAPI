using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.Role;

namespace SimpleStoreAPI.Interfaces
{
    public interface IRoleService
    {
        Task<Result<RoleResponseDto>> CreateAsync(CreateRoleDto roleDto);
        Task<IEnumerable<RoleResponseDto>> GetAllAsync();
        Task<Result<RoleResponseDto>> GetByIdAsync(string id);
        Task<Result<RoleResponseDto>> UpdateAsync(string id, UpdateRoleDto roleDto);
        Task<Result> DeleteAsync(string id);
    }
}
