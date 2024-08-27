using Authentication_System.Filters;
using Core.Domain.Entities;
using Core.DTO.AccountsDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_System.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _userServices;

        public UsersController(IUsersServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ByUsername(GetUserByUsernameDto getUserByUsernameDto)
        {
            try
            {
                ApplicationUser user = await _userServices.GetUserByUsername(getUserByUsernameDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ByToken(GetUserByTokenDto getUserByTokenDto)
        {
            try
            {
                ApplicationUser user = await _userServices.GetUserByToken(getUserByTokenDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Username(UpdateUsernameDto updateUsernameDto)
        {
            try
            {
                await _userServices.UpdateUsername(updateUsernameDto);
                return Ok("Username updated successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Email(UpdateEmailDto updateEmailDto)
        {
            try
            {
                await _userServices.UpdateEmail(updateEmailDto);
                return Ok("Email updated successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Password(UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                await _userServices.UpdatePassword(updatePasswordDto);
                return Ok("Password updated successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DeleteUserAccountDto deleteUserAccountDto)
        {
            try
            {
                await _userServices.DeleteUserAccount(deleteUserAccountDto);
                return Ok("User account was deleted successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }
    }
}
