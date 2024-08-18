using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RefreshTokenDtos
{
    public class RefreshTokenDto
    {
        [Required]
        public string? OldToken { get; set; }

        [Required]
        public string? RefreshToken { get; set; }
    }
}
