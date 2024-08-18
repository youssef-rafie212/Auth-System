using System.ComponentModel.DataAnnotations;

namespace Core.DTO._2faDtos
{
    public class Disable2faDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
