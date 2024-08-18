using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Entities
{
    public class APIKey
    {
        [Key]
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public Guid TenantId { get; set; } = Guid.NewGuid();
    }
}
