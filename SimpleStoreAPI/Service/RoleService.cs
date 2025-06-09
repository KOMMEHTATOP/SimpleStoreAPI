using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleStoreAPI.DTOs;
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

        public async Task<Result<RoleResponseDto>> CreateAsync(CreateRoleDto roleDto)
        {
            var existsRole = await _roleManager.RoleExistsAsync(roleDto.Name);

            if (existsRole)
            {
                return Result<RoleResponseDto>.Failed("Role with this name already exists");
            }

            ApplicationRole newRole = new ApplicationRole
            {
                Name = roleDto.Name, Description = roleDto.Description
            };

            var createRole = await _roleManager.CreateAsync(newRole);

            if (!createRole.Succeeded)
            {
                var errors = createRole.Errors.Select(e => e.Description).ToList();
                
                return Result<RoleResponseDto>.Failed(errors);
            }
            
            var result = RoleMapper.ApplicationRoleToRoleResponseDto(newRole);
            
            return Result<RoleResponseDto>.Success(result);
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return roles.Select(RoleMapper.ApplicationRoleToRoleResponseDto);
        }

        public async Task<Result<RoleResponseDto>> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Result<RoleResponseDto>.Failed("Id cannot be empty");
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return Result<RoleResponseDto>.Failed($"Role with ID {id} does not exist");
            }

            var roleDto = RoleMapper.ApplicationRoleToRoleResponseDto(role);
            return Result<RoleResponseDto>.Success(roleDto);
        }

        public async Task<Result<RoleResponseDto>> UpdateAsync(string id, UpdateRoleDto roleDto)
        {
            var existsRole = await _roleManager.FindByIdAsync(id);

            if (existsRole == null)
            {
                return Result<RoleResponseDto>.Failed($"Role with ID {id} does not exist");
            }

            if (string.IsNullOrEmpty(roleDto.Name))
            {
                return Result<RoleResponseDto>.Failed("Role name cannot be empty");
            }

            if (existsRole.Name != roleDto.Name)
            {
                var checkRoleName = await _roleManager.RoleExistsAsync(roleDto.Name);

                if (checkRoleName)
                {
                    return Result<RoleResponseDto>.Failed($"Role with name {roleDto.Name} already exists");
                } 
            }
            
            existsRole.Name = roleDto.Name;
            existsRole.Description = roleDto.Description;
            var updateRole = await _roleManager.UpdateAsync(existsRole);

            if (!updateRole.Succeeded)
            {
                var errors = updateRole.Errors.Select(e => e.Description).ToList();
                return Result<RoleResponseDto>.Failed(errors);
            }

            var roleToDto = RoleMapper.ApplicationRoleToRoleResponseDto(existsRole);
            return Result<RoleResponseDto>.Success(roleToDto);
        }

        public async Task<Result> DeleteAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return Result.Failed($"Role with ID {id} does not exist");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Result.Failed(errors);
            }

            return Result.Success();
        }

    }
}
