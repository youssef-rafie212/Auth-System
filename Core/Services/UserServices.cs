using Core.Domain.Entities;
using Core.DTO.AccountsDtos;
using Core.Helpers;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Core.Services
{
    public class UserServices : IUsersServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtServices _jwtServices;
        private readonly ValidationHelpers _helpers;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserServices(UserManager<ApplicationUser> userManager, IJwtServices jwtServices, ValidationHelpers helpers, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
            _helpers = helpers;
            _contextAccessor = contextAccessor;
        }

        public async Task DeleteUserAccount(DeleteUserAccountDto deleteUserAccountDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(deleteUserAccountDto.UserName!);
            if (user == null) throw new Exception("User doesn't exist.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedTenantId(tenantId, user!);

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task<ApplicationUser> GetUserByToken(GetUserByTokenDto getUserByTokenDto)
        {
            bool validToken = await _jwtServices.ValidateToken(getUserByTokenDto.Token!);
            if (!validToken) throw new Exception("Invalid or expired token.");

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken token = handler.ReadJwtToken(getUserByTokenDto.Token);
            string userId = token.Subject;

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("No user found.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedTenantId(tenantId, user!);

            return user;
        }

        public async Task<ApplicationUser> GetUserByUsername(GetUserByUsernameDto getUserByUsernameDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(getUserByUsernameDto.Username!);
            if (user == null) throw new Exception("No user found.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedTenantId(tenantId, user!);

            return user;
        }

        public async Task UpdateEmail(UpdateEmailDto updateEmailDto)
        {
            // TODO: Send a validation code to the user email before updating.

            ApplicationUser? user = await _userManager.FindByEmailAsync(updateEmailDto.OldEmail!);
            if (user == null) throw new Exception("No user found.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedTenantId(tenantId, user!);

            user.Email = updateEmailDto.NewEmail;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateNormalizedEmailAsync(user);
        }

        public async Task UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(updatePasswordDto.Username!);
            if (user == null) throw new Exception("User doesn't exist.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedTenantId(tenantId, user!);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, updatePasswordDto.OldPassword!, updatePasswordDto.NewPassword!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task UpdateUsername(UpdateUsernameDto updateUsernameDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(updateUsernameDto.OldUsername!);
            if (user == null) throw new Exception("User doesn't exist.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedTenantId(tenantId, user!);

            user.UserName = updateUsernameDto.NewUsername!;
            await _userManager.UpdateAsync(user);
            await _userManager.UpdateNormalizedUserNameAsync(user);
        }
    }
}
