using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class GetUserByTokenDto
    {
        [Required]
        public string? Token { get; set; }
    }
}
