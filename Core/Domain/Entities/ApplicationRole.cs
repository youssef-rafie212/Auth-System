﻿using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string? TenantId { get; set; }
    }
}
