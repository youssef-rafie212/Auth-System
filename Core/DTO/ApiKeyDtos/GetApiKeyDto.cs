using System.ComponentModel.DataAnnotations;

namespace Core.DTO.ApiKeyDtos
{
    public class GetApiKeyDto
    {
        [Required]
        public string? ClientName { get; set; }
    }
}
