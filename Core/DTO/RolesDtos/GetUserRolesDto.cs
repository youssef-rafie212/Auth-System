using System.ComponentModel.DataAnnotations;

namespace Core.DTO.RolesDtos
{
    internal class GetUserRolesDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
