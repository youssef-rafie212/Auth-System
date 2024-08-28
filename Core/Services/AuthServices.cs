using Core.Domain.Entities;
using Core.DTO._2faDtos;
using Core.DTO.AccountsDtos;
using Core.DTO.EmailConfirmationDtos;
using Core.Helpers;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtServices _jwtServices;
        private readonly ValidationHelpers _helpers;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthServices(UserManager<ApplicationUser> userManager, IJwtServices jwtServices, ValidationHelpers helpers, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _jwtServices = jwtServices;
            _helpers = helpers;
            _contextAccessor = contextAccessor;
        }

        public async Task<ConfirmEmailResponseDto> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(confirmEmailDto.Email!);
            if (user == null) throw new Exception("No user found with this email.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            if (user.EmailConfirmed) throw new Exception("User already confirmed their email.");

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return new ConfirmEmailResponseDto
            {
                EmailConfirmationCode = code,
                UserEmail = user.Email
            };
        }

        public async Task Disable2fa(Disable2faDto disable2faDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(disable2faDto.Username!);
            if (user == null) throw new Exception("No user found with the provided username.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!result.Succeeded) throw new Exception("Failed to disable 2fa please try again later.");
        }

        public async Task Enable2fa(Enable2faDto enable2faDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(enable2faDto.Username!);
            if (user == null) throw new Exception("No user found with the provided username.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            if (!user.EmailConfirmed) throw new Exception("User must confirm their email before enabling 2FA.");

            IdentityResult result = await _userManager.SetTwoFactorEnabledAsync(user, true);
            if (!result.Succeeded) throw new Exception("Failed to enable 2fa please try again later.");
        }

        public async Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email!);
            if (user == null) throw new Exception("No user found with this email.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new ForgotPasswordResponseDto
            {
                PasswordResetToken = resetToken,
                UserEmail = user.Email,
            };
        }

        public async Task<RegisterResponseDto> Register(RegisterDto registerDto)
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
                throw new Exception(errors);
            }

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);
            user.TenantId = _contextAccessor.HttpContext!.Items["TenantId"] as string;

            await _userManager.UpdateAsync(user);

            return new RegisterResponseDto
            {
                RefreshToken = refreshToken,
                Token = token,
            };
        }

        public async Task ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(resetPasswordDto.Email!);
            if (user == null) throw new Exception("No user found with this email.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.ResetToken!, resetPasswordDto.NewPassword!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        public async Task<SigninResponseDto> Signin(SiginDto siginDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(siginDto.Username!);
            if (user == null) throw new Exception("Wrong credentials.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            bool correctPassword = await _userManager.CheckPasswordAsync(user, siginDto.Password!);
            if (!correctPassword) throw new Exception("Wrong credentials.");

            if (user.TwoFactorEnabled)
            {
                string code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                return new SigninWith2faResponseDto
                {
                    TwoFactorsAuthenticationCode = code,
                    UserEmail = user.Email,
                };
            }

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);

            await _userManager.UpdateAsync(user);

            return new SigninWithout2faResponseDto { RefreshToken = refreshToken, Token = token };
        }

        public async Task Signout(SignoutDto signoutDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(signoutDto.Username!);
            if (user == null) throw new Exception("No user found.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        public async Task<Validate2faResponseDto> Validate2fa(Validate2faDto validate2faDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(validate2faDto.Username!);
            if (user == null) throw new Exception("No user found with the provided username.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            bool succeeded = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", validate2faDto.Code!);
            if (!succeeded) throw new Exception("Invalid 2fa code.");

            string token = await _jwtServices.GenerateToken(user);
            string refreshToken = _jwtServices.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddMonths(1);

            await _userManager.UpdateAsync(user);

            return new Validate2faResponseDto
            {
                RefreshToken = refreshToken,
                Token = token,
            };
        }

        public async Task ValidateConfirmEmail(ValidateConfirmEmailDto validateConfirmEmailDto)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(validateConfirmEmailDto.Username!);
            if (user == null) throw new Exception("No user found with the provided username.");

            // Validate that the user belongs to the current client.
            string tenantId = (_contextAccessor.HttpContext!.Items["TenantId"] as string)!;
            _helpers.ThrowIfUnmatchedUserTenantId(tenantId, user!);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, validateConfirmEmailDto.Code!);

            if (!result.Succeeded)
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }
    }
}
