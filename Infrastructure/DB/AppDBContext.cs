using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
{
    public class AppDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<APIKey> APIKeys { get; set; }

        public AppDBContext(DbContextOptions options) : base(options) { }
    }
}
