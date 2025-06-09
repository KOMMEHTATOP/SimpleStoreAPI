using SimpleStoreAPI.DTOs.Role;

namespace SimpleStoreAPI.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResult> CreateAsync(RoleDto roleDto);
        Task<RoleResult> GetByIdAsync(string id);
        Task<IEnumerable<RoleResult>> GetAllAsync();
        Task<RoleResult> UpdateAsync(string id, RoleDto roleDto);
        Task<RoleResult> DeleteAsync(string id);
    }
}
