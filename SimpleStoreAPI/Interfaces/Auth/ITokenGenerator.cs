using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Interfaces.Auth
{
    public interface ITokenGenerator
    {
        public string GenerateToken(ApplicationUser user);
    }
}
