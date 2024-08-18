using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    public class RemoveUserFromRoleDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? RoleName { get; set; }
    }
}
