using System.ComponentModel.DataAnnotations;

namespace Core.DTO._2faDtos
{
    public class Verify2faDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Code { get; set; }
    }
}
