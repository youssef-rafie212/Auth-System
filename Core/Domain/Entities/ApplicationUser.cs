﻿using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? TenantId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
