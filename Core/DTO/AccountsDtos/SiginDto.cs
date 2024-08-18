using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class SiginDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
