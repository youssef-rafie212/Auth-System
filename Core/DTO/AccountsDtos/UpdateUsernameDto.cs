using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class UpdateUsernameDto
    {
        [Required]
        public string? OldUsername { get; set; }

        [Required]
        public string? NewUsername { get; set; }
    }
}
