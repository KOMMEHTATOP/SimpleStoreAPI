namespace SimpleStoreAPI.DTOs.Auth
{
    public class LoginResult : AuthResult
    {
        public string Token { get; set; } = null!;
    }
}
