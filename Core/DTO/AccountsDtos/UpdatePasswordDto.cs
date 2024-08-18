using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class UpdatePasswordDto
    {
        [Required]
        string? Username { get; set; }

        [Required]
        string? OldPassword { get; set; }

        [Required]
        string? NewPassword { get; set; }
    }
}
