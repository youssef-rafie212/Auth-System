using Authentication_System.Filters;
using Core.DTO.JwtDtos;
using Core.DTO.RefreshTokenDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_System.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class TokensController : ControllerBase
    {
        private readonly IJwtServices _jwtServices;

        public TokensController(IJwtServices jwtServices)
        {
            _jwtServices = jwtServices;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Validate(ValidateTokenDto validateTokenDto)
        {
            return Ok(await _jwtServices.ValidateToken(validateTokenDto.Token!));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto refreshTokenRequestDto)
        {
            try
            {
                return Ok(await _jwtServices.RefreshToken(refreshTokenRequestDto));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }
    }
}
