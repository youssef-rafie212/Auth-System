using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
