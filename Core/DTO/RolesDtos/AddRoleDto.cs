using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    public class AddRoleDto
    {
        [Required]
        public string? RoleName { get; set; }
    }
}
