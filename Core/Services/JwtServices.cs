using Core.Domain.Entities;
using Core.DTO.RefreshTokenDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtServices(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            DateTime expires = DateTime.UtcNow.AddMinutes(30);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTHSYS_DEV_JWT_SECRET")!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat , DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name , user.UserName!),
                new Claim(ClaimTypes.NameIdentifier , user.UserName!),
                new Claim(ClaimTypes.Email , user.Email!)
            ];

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            JwtSecurityTokenHandler handler = new();

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = Environment.GetEnvironmentVariable("BEQALTK_DEV_JWT_ISSUER"),
                Expires = expires,
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(descriptor);

            string jwt = handler.WriteToken(token);

            return jwt;
        }

        public Task<string> RefreshToken(RefreshTokenRequestDto refreshTokenDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var result = await tokenHandler.ValidateTokenAsync(token,
                new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("AUTHSYS_DEV_JWT_ISSUER")!,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTHSYS_DEV_JWT_SECRET")!)
                        ),
                }
            );

            return result.IsValid;
        }
    }
}
