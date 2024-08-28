using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class GetUserByRefreshTokenDto
    {
        [Required]
        public string? RefreshToken { get; set; }
    }
}
