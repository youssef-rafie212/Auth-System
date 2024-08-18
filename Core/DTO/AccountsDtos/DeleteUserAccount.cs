using System.ComponentModel.DataAnnotations;

namespace Core.DTO.AccountsDtos
{
    public class DeleteUserAccount
    {
        [Required]
        public string? UserName { get; set; }
    }
}
