using System.ComponentModel.DataAnnotations;

namespace Core.DTO.ApiKeyDtos
{
    public class DeactivateApiKeyDto
    {
        [Required]
        public string? Key { get; set; }
    }
}
