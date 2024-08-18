using System.ComponentModel.DataAnnotations;

namespace Core.DTO._2faDtos
{
    public class Enable2faDto
    {
        [Required]
        public string? Username { get; set; }
    }
}
