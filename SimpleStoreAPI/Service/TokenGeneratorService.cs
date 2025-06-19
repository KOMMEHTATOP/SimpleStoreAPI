using Microsoft.IdentityModel.Tokens;
using SimpleStoreAPI.Interfaces.Auth;
using SimpleStoreAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleStoreAPI.Service
{
    public class TokenGeneratorService : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGeneratorService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user,  IEnumerable<string> roles)
        {
            //список утверждений о пользователе
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id), 
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            //получаем ключ подписи
            var jwtKey = _configuration["Jwt:Key"];
            var keyInBytes = Encoding.UTF8.GetBytes(jwtKey);
            var securityKey = new SymmetricSecurityKey(keyInBytes);

            //соединяем ключ с алгоритмом шифрования
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Собираем токен из подготовленных данных выше СКОБКИ КРУГЛЫЕ!!!!!!!
            int expMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"]);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddMinutes(expMinutes),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(descriptor);
            var tokenString = handler.WriteToken(securityToken);
            return tokenString;

        }
    }
}
