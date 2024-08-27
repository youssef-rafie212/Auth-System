using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class UpdatePasswordDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? OldPassword { get; set; }

        [Required]
        public string? NewPassword { get; set; }
    }
}
