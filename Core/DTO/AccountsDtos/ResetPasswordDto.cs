using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? NewPassword { get; set; }
    }
}
