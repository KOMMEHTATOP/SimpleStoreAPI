using SimpleStoreAPI.DTOs.Auth;

namespace SimpleStoreAPI.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDto registerDto);
        Task<LoginResult> LoginAsync(LoginDto loginDto);
    }
}
