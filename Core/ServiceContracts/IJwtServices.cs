using Core.Domain.Entities;
using Core.DTO.RefreshTokenDtos;

namespace Core.ServiceContracts
{
    public interface IJwtServices
    {
        Task<string> GenerateToken(ApplicationUser user);

        string GenerateRefreshToken();

        Task<bool> ValidateToken(string token);

        Task<string> RefreshToken(RefreshTokenRequestDto refreshTokenDto);
    }
}
