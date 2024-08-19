using System.ComponentModel.DataAnnotations;

namespace Core.DTO.ApiKeyDtos
{
    public class ActivateApiKeyDto
    {
        [Required]
        public string? Key { get; set; }
    }
}
