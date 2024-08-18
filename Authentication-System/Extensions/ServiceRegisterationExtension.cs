using Authentication_System.Filters;
using Core.Domain.Entities;
using Core.Domain.RepositoryContracts;
using Core.ServiceContracts;
using Core.Services;
using Infrastructure.DB;
using Infrastructure.Externals;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
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

            builder.Services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
            });

            builder.Services.AddScoped<IApiKeysServices, ApiKeysServices>();
            builder.Services.AddScoped<IApiKeysRepository, ApiKeysRepository>();
            builder.Services.AddScoped<IJwtServices, JwtServices>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<ApiKeyAuthFilter>();

            return builder;
        }
    }
}
