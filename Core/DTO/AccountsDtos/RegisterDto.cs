using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(30)]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? PasswordConfirmation { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }
}