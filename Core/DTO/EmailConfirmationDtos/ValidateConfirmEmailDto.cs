using System.ComponentModel.DataAnnotations;

namespace Core.DTO.EmailConfirmationDtos
{
    public class ValidateConfirmEmailDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
