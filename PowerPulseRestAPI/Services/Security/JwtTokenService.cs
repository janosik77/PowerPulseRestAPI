using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PowerPulseRestAPI.Services.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(long userId, string login, string email, string role, long employeeId)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var key = jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
            var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured.");
            var audience = jwtSection["Audience"] ?? throw new InvalidOperationException("JWT Audience not configured.");
            var expiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "120");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("employeeId", employeeId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
