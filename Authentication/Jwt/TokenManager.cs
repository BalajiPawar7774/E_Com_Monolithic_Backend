using E_Com_Monolithic.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Com_Monolithic.Authentication.Jwt
{
    public class TokenManager : ITokenManager
    {

        private readonly SymmetricSecurityKey _key;
        public TokenManager(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:SecretKey"]));
        }

        public Task<string> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // Subject (user ID)
                new Claim(JwtRegisteredClaimNames.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Issued at
                new Claim(ClaimTypes.Role, user.Role)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
                Issuer = "E-Com-Monolithic", // Add issuer
                Audience = "E-Com-Front-End" // Add audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
      
    }
}
