using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    public class UpdateRoleNameDto
    {
        [Required]
        public string? OldRoleName { get; set; }

        [Required]
        public string? NewRoleName { get; set; }
    }
}
