using Authentication_System.Filters;
using Core.Domain.Entities;
using Core.DTO.AccountsDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_System.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtServices _jwtServices;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtServices jwtServices)
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            ApplicationUser? userWithTheSameEmail = await _userManager.FindByEmailAsync(registerDto.Email!);
            if (userWithTheSameEmail != null) return Problem("Email already exists.", statusCode: 400);

            ApplicationUser user = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone
            };

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);
            user.TenantId = HttpContext.Items["TenantId"] as string;

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                return Problem(errors, statusCode: 400);
            }

            return Ok(new
            {
                token,
                refreshToken
            });
        }
    }
}
