using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopOnline.API.Repositories.Utilities
{
    public static class JwtGenerator
    {
        public static string GenerateUserToken(string username, string userId)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("userId", userId)
            };

            return GenerateToken(claims, DateTime.UtcNow.AddDays(1));
        }
        public static string GenerateToken(Claim[] claims, DateTime expiryTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiryTime,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

}
