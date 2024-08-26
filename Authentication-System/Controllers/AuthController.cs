using Authentication_System.Filters;
using Core.Domain.Entities;
using Core.DTO._2faDtos;
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

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IJwtServices jwtServices
            )
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.Username,
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                return Problem(errors, statusCode: 400);
            }

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);
            user.TenantId = HttpContext.Items["TenantId"] as string;

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token,
                refreshToken
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Signin(SiginDto siginDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(siginDto.Username!);
            if (user == null) return Problem("Wrong credentials.", statusCode: 400);

            bool correctPassword = await _userManager.CheckPasswordAsync(user, siginDto.Password!);
            if (!correctPassword) return Problem("Wrong credentials.", statusCode: 400);

            if (user.TwoFactorEnabled)
            {
                string code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                return Ok(new
                {
                    TwoFactorsAuthenticationCode = code,
                    UserEmail = user.Email,
                });
            }

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token,
                refreshToken
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Enable2fa(Enable2faDto enable2faDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(enable2faDto.Username!);
            if (user == null) return Problem("No user found with the provided username.", statusCode: 400);

            IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, true);
            if (!result.Succeeded) return Problem("Failed to enable 2fa please try again later.", statusCode: 400);

            return Ok($"2fa is now enabled for '{user.UserName}'");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Disable2fa(Disable2faDto disable2faDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(disable2faDto.Username!);
            if (user == null) return Problem("No user found with the provided username.", statusCode: 400);

            IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!result.Succeeded) return Problem("Failed to disable 2fa please try again later.", statusCode: 400);

            return Ok($"2fa is now disabled for '{user.UserName}'");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Validate2fa(Validate2faDto validate2faDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(validate2faDto.Username!);
            if (user == null) return Problem("No user found with the provided username.", statusCode: 400);

            bool succeeded = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", validate2faDto.Code!);
            if (!succeeded) return Problem("Invalid 2fa code.", statusCode: 400);

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token,
                refreshToken
            });
        }
    }
}
