using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class GetUserByUsernameDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
