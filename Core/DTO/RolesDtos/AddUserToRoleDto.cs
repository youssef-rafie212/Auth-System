using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    public class AddUserToRoleDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? RoleName { get; set; }
    }
}
