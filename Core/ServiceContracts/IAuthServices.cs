using Core.DTO._2faDtos;
using Core.DTO.AccountsDtos;
using Core.DTO.EmailConfirmationDtos;

namespace Core.ServiceContracts
{
    public interface IAuthServices
    {
        public Task<RegisterResponseDto> Register(RegisterDto registerDto);

        public Task<SigninResponseDto> Signin(SiginDto siginDto);

        public Task Signout(SignoutDto signoutDto);

        public Task Enable2fa(Enable2faDto enable2faDto);

        public Task Disable2fa(Disable2faDto disable2faDto);

        public Task<Validate2faResponseDto> Validate2fa(Validate2faDto validate2faDto);

        public Task<ConfirmEmailResponseDto> ConfirmEmail(ConfirmEmailDto confirmEmailDto);

        // Will throw an exception in case of wrong confirmation token.
        public Task ValidateConfirmEmail(ValidateConfirmEmailDto validateConfirmEmailDto);

        public Task<ForgotPasswordResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto);

        public Task ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
