using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class UpdateUsernameDto
    {
        [Required]
        string? OldUsername { get; set; }

        [Required]
        string? NewUsername { get; set; }
    }
}
