using Core.Domain.Entities;
using Core.DTO.RefreshTokenDtos;
using Core.Helpers;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ServicesHelpers _servicesHelpers;

        public JwtServices(UserManager<ApplicationUser> userManager, ServicesHelpers servicesHelpers)
        {
            _userManager = userManager;
            _servicesHelpers = servicesHelpers;
        }

        public string GenerateRefreshToken()
        {
            return _servicesHelpers.GenerateUniqueString();
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
                Issuer = Environment.GetEnvironmentVariable("AUTHSYS_DEV_JWT_ISSUER"),
                Expires = expires,
                SigningCredentials = credentials,
            };

            var token = handler.CreateToken(descriptor);

            string jwt = handler.WriteToken(token);

            return jwt;
        }

        public async Task<RefreshTokenResponseDto> RefreshToken(RefreshTokenRequestDto refreshTokenDto)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken expiredToken = handler.ReadJwtToken(refreshTokenDto.OldToken);

            string userId = expiredToken.Subject;
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null) throw new ArgumentException("No user is associated with this token (old token).");

            if (user.RefreshToken == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiresAt < DateTime.UtcNow)
                throw new ArgumentException("Invalid or expired refresh token.");

            string newToken = await GenerateToken(user);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);
            await _userManager.UpdateAsync(user);

            return new RefreshTokenResponseDto { NewRefreshToken = newRefreshToken, NewToken = newToken };
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
