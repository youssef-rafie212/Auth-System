using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class UpdateEmailDto
    {
        [Required]
        [EmailAddress]
        string? OldEmail { get; set; }

        [Required]
        [EmailAddress]
        string? NewEmail { get; set; }
    }
}
