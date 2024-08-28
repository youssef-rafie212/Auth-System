using Core.Domain.Entities;
using Core.DTO.RolesDtos;
using Core.Helpers;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class RoleServices : IRolesServices
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ValidationHelpers _helpers;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleServices(ValidationHelpers helpers,
            RoleManager<ApplicationRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _helpers = helpers;
            _roleManager = roleManager;
            _userManager = userManager;
            _contextAccessor = httpContextAccessor;
        }

        public async Task AddRole(AddRoleDto addRoleDto)
        {
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;

            ApplicationRole role = new();
            role.Name = addRoleDto.RoleName;
            role.TenantId = tenantId;

            IdentityResult result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task AddUserToRole(AddUserToRoleDto addUserToRoleDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(addUserToRoleDto.Username!);
            if (user == null) throw new Exception("User doesn't exist.");

            ApplicationRole? role = await _roleManager.FindByNameAsync(addUserToRoleDto.RoleName!);
            if (role == null) throw new Exception("Role doesn't exist.");

            // Validate that both the user and role belong to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user);
            _helpers.ThrowIfUnmatchedRoleTenantId(tenantId, role);

            IdentityResult result = await _userManager.AddToRoleAsync(user, role.Name!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task DeleteRole(DeleteRoleDto deleteRoleDto)
        {
            ApplicationRole? role = await _roleManager.FindByNameAsync(deleteRoleDto.RoleName!);
            if (role == null) throw new Exception("Role doesn't exist.");

            // Validate that the role belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedRoleTenantId(tenantId, role);

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task<IList<string>> GetUserRoles(GetUserRolesDto getUserRolesDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(getUserRolesDto.Username!);
            if (user == null) throw new Exception("User doesn't exist.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user);

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            return userRoles;
        }

        public async Task RemoveUserFromRole(RemoveUserFromRoleDto removeUserFromRoleDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(removeUserFromRoleDto.Username!);
            if (user == null) throw new Exception("User doesn't exist.");

            ApplicationRole? role = await _roleManager.FindByNameAsync(removeUserFromRoleDto.RoleName!);
            if (role == null) throw new Exception("Role doesn't exist.");

            // Validate that both the user and role belong to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user);
            _helpers.ThrowIfUnmatchedRoleTenantId(tenantId, role);

            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role.Name!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task UpdateRoleName(UpdateRoleNameDto updateRoleNameDto)
        {
            ApplicationRole? role = await _roleManager.FindByNameAsync(updateRoleNameDto.OldRoleName!);
            if (role == null) throw new Exception("Role doesn't exist.");

            // Validate that the role belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedRoleTenantId(tenantId, role);

            role.Name = updateRoleNameDto.NewRoleName!;

            await _roleManager.UpdateAsync(role);
            await _roleManager.UpdateNormalizedRoleNameAsync(role);
        }
    }
}
