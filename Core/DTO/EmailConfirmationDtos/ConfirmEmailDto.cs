using System.ComponentModel.DataAnnotations;

namespace Core.DTO.EmailConfirmationDtos
{
    public class ConfirmEmailDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
