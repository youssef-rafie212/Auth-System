using System.ComponentModel.DataAnnotations;

namespace Core.DTO.JwtDtos
{
    public class ValidateTokenDto
    {
        [Required]
        public string? Token { get; set; }
    }
}
