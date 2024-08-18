using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    public class DeleteRoleDto
    {
        [Required]
        public string? RoleName { get; set; }
    }
}
