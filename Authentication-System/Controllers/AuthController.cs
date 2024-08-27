using Authentication_System.Filters;
using Core.DTO._2faDtos;
using Core.DTO.AccountsDtos;
using Core.DTO.EmailConfirmationDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_System.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                RegisterResponseDto result = await _authServices.Register(registerDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Signin(SiginDto siginDto)
        {
            try
            {
                SigninResponseDto result = await _authServices.Signin(siginDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Signout(SignoutDto signoutDto)
        {
            try
            {
                await _authServices.Signout(signoutDto);
                return Ok("User signed out successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Enable2fa(Enable2faDto enable2faDto)
        {
            try
            {
                await _authServices.Enable2fa(enable2faDto);
                return Ok($"2FA enabled for the user '{enable2faDto.Username}'.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Disable2fa(Disable2faDto disable2faDto)
        {
            try
            {
                await _authServices.Disable2fa(disable2faDto);
                return Ok($"2FA disabled for the user '{disable2faDto.Username}'.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Validate2fa(Validate2faDto validate2faDto)
        {
            try
            {
                Validate2faResponseDto result = await _authServices.Validate2fa(validate2faDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            try
            {
                ConfirmEmailResponseDto result = await _authServices.ConfirmEmail(confirmEmailDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ValidateConfirmEmail(ValidateConfirmEmailDto validateConfirmEmailDto)
        {
            try
            {
                await _authServices.ValidateConfirmEmail(validateConfirmEmailDto);
                return Ok($"Email confirmed for the user '{validateConfirmEmailDto.Username}'");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                ForgotPasswordResponseDto result = await _authServices.ForgotPassword(forgotPasswordDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                await _authServices.ResetPassword(resetPasswordDto);
                return Ok($"Password reset successfully for user with email'{resetPasswordDto.Email}'");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }
    }
}
