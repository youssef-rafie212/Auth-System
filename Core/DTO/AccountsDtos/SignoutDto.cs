using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class SignoutDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
