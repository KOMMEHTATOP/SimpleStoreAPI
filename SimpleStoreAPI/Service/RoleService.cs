using Microsoft.AspNetCore.Identity;
using SimpleStoreAPI.DTOs.Role;
using SimpleStoreAPI.Interfaces;
using SimpleStoreAPI.Mappers;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<RoleResult> CreateAsync(RoleDto roleDto)
        {
            var existsRole = await _roleManager.RoleExistsAsync(roleDto.Name);
            if (existsRole)
            {
                return RoleResult.Failed("Role with this name already exists");
            }

            ApplicationRole newRole = new ApplicationRole { Name = roleDto.Name, Description = roleDto.Description };

            var createRole = await _roleManager.CreateAsync(newRole);
            if (!createRole.Succeeded)
            {
                var errors = createRole.Errors.Select(e => e.Description).ToList();
                return RoleResult.Failed(errors);
            }

            var role = await _roleManager.FindByNameAsync(newRole.Name);
            if (role == null)
            {
                return RoleResult.Failed("Created role not found");
            }

            var result = RoleMapper.ApplicationRoleToDto(role);
            return RoleResult.Success(result);
        }

        public Task<RoleResult> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoleResult>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RoleResult> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<RoleResult> UpdateAsync(string id, RoleDto roleDto)
        {
            throw new NotImplementedException();
        }
    }
}
