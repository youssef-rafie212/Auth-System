using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class UpdateEmailDto
    {
        [Required]
        [EmailAddress]
        public string? OldEmail { get; set; }

        [Required]
        [EmailAddress]
        public string? NewEmail { get; set; }
    }
}
