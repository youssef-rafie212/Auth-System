using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities
{
    public class APIKey
    {
        [Key]
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? ClientName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string? TenantId { get; set; }
    }
}
