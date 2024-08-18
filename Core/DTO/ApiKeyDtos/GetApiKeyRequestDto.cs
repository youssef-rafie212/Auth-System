using System.ComponentModel.DataAnnotations;

namespace Core.DTO.ApiKeyDtos
{
    public class GetApiKeyRequestDto
    {
        [Required]
        public string? ClientName { get; set; }
    }
}
