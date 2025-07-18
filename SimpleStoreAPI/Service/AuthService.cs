﻿using Microsoft.AspNetCore.Identity;
using SimpleStoreAPI.DTOs.Auth;
using SimpleStoreAPI.Interfaces.Auth;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signIdManager;
        private readonly ITokenGenerator _tokenGenerator;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _signIdManager = signInManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                Email = registerDto.Email, UserName = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResult
                {
                    IsSuccess = false, Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "Customer");
            return new AuthResult
            {
                IsSuccess = true, Message = "Пользователь успешно зарегистрирован."
            };
        }

        public async Task<LoginResult> LoginAsync(LoginDto loginDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if (existingUser == null)
            {
                return new LoginResult
                {
                    IsSuccess = false, Message = "User not found"
                };
            }

            var checkLoginPassword = await _signIdManager
                .CheckPasswordSignInAsync(existingUser, loginDto.Password, lockoutOnFailure: false);

            if (!checkLoginPassword.Succeeded)
            {
                return new LoginResult
                {
                    IsSuccess = false, Message = "Login, password not valide"
                };
            }

            var userRoles = await _userManager.GetRolesAsync(existingUser);

            var tokenString = _tokenGenerator.GenerateToken(existingUser, userRoles);

            return new LoginResult
            {
                IsSuccess = true, Message = "Ok", Token = tokenString
            };
        }
    }
}
