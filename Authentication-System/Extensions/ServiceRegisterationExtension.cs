using Core.Domain.Entities;
using Infrastructure.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = builder.Environment.IsDevelopment() ? false : true;

                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("AUTHSYS_DEV_JWT_ISSUER")!,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTHSYS_DEV_JWT_SECRET")!)
                        ),
                };
            });

            return builder;
        }
    }
}
