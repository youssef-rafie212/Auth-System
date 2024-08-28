using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    public class GetUserRolesDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
