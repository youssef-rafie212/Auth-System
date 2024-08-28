using Authentication_System.Filters;
using Core.DTO.RolesDtos;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_System.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class RolesController : ControllerBase
    {
        private readonly IRolesServices _rolesServices;

        public RolesController(IRolesServices rolesServices)
        {
            _rolesServices = rolesServices;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UserRoles(GetUserRolesDto getUserRolesDto)
        {
            try
            {
                return Ok(await _rolesServices.GetUserRoles(getUserRolesDto));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole(AddRoleDto addRoleDto)
        {
            try
            {
                await _rolesServices.AddRole(addRoleDto);
                return Ok($"Role '{addRoleDto.RoleName}' was added successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleDto addUserToRoleDto)
        {
            try
            {
                await _rolesServices.AddUserToRole(addUserToRoleDto);
                return Ok($"User '{addUserToRoleDto.Username}' was added to the role '{addUserToRoleDto.RoleName}'.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteRole(DeleteRoleDto deleteRoleDto)
        {
            try
            {
                await _rolesServices.DeleteRole(deleteRoleDto);
                return Ok($"Role '{deleteRoleDto.RoleName}' was deleted successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveUserFromRole(RemoveUserFromRoleDto removeUserFromRoleDto)
        {
            try
            {
                await _rolesServices.RemoveUserFromRole(removeUserFromRoleDto);
                return Ok($"User '{removeUserFromRoleDto.Username}' was removed from the role '{removeUserFromRoleDto.RoleName}'.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> RoleName(UpdateRoleNameDto updateRoleNameDto)
        {
            try
            {
                await _rolesServices.UpdateRoleName(updateRoleNameDto);
                return Ok($"Role name was updated successfully.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
        }
    }
}
