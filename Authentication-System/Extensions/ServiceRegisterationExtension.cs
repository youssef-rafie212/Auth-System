using Core.Domain.Entities;
using Infrastructure.DB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication_System.Extensions
{
    public static class ServiceRegisterationExtension
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("AUTHSYS_DEV_DB_DEFAULT_URL"));
            });

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDBContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDBContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, AppDBContext, Guid>>();

            return builder;
        }
    }
}
