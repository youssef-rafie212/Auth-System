using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class GetUserByUsername
    {
        [Required]
        public string? Username { get; set; }
    }
}
