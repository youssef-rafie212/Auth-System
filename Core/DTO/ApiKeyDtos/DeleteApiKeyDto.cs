using System.ComponentModel.DataAnnotations;

namespace Core.DTO.ApiKeyDtos
{
    public class DeleteApiKeyDto
    {
        [Required]
        public string? Key { get; set; }
    }
}
