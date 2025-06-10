using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleStoreAPI.Data;
using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.User;
using SimpleStoreAPI.Interfaces;
using SimpleStoreAPI.Mappers;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Service;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Result<UserResponseDto>> GetByIdAsync(string id)
    {
        var existingUser = await _userManager.FindByIdAsync(id);

        if (existingUser == null)
        {
            return Result<UserResponseDto>.Failed("User not found");
        }

        var userRoles = await _userManager.GetRolesAsync(existingUser);
        var result = UserMapper.ApplicationUserToDto(existingUser, userRoles);

        return Result<UserResponseDto>.Success(result);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var allRoles = await _context.Roles.ToDictionaryAsync(r => r.Id, r => r.Name!);

        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ToListAsync();

        var result = users.Select(user => new UserResponseDto
        {
            Id = user.Id,
            Name = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            Roles = user.UserRoles
                .Where(ur => allRoles.ContainsKey(ur.RoleId))
                .Select(ur => allRoles[ur.RoleId]).ToList()
        }).ToList();

        return result;
    }

    public async Task<Result<UserResponseDto>> UpdateAsync(string id, UpdateUserDto userDto)
    {
        var existingUser = await _userManager.FindByIdAsync(id);

        if (existingUser == null)
        {
            return Result<UserResponseDto>.Failed("User not found");
        }

        existingUser.UserName = userDto.Name;
        existingUser.Email = userDto.Email!;
        existingUser.PhoneNumber = userDto.PhoneNumber;

        var currentUserRoles = await _userManager.GetRolesAsync(existingUser);
        await _userManager.RemoveFromRolesAsync(existingUser, currentUserRoles);
        await _userManager.AddToRolesAsync(existingUser, userDto.Roles);

        var updateUser = await _userManager.UpdateAsync(existingUser);

        if (!updateUser.Succeeded)
        {
            return Result<UserResponseDto>.Failed("There was an error updating the user");
        }

        var updatedRoles = await _userManager.GetRolesAsync(existingUser);
        var result = UserMapper.ApplicationUserToDto(existingUser, updatedRoles);

        return Result<UserResponseDto>.Success(result);
    }

    public async Task<Result> DeleteAsync(string id)
    {
        var existingUser = await _userManager.FindByIdAsync(id);

        if (existingUser == null)
        {
            return Result.Failed("User not found");
        }

        var deleteUser = await _userManager.DeleteAsync(existingUser);

        if (!deleteUser.Succeeded)
        {
            return Result.Failed("There was an error deleting the user");
        }

        return Result.Success();
    }
}
