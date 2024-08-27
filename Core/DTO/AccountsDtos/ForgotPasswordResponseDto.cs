namespace Core.DTO.AccountsDtos
{
    public class ForgotPasswordResponseDto
    {
        public string? PasswordResetToken { get; set; }
        public string? UserEmail { get; set; }
    }
}
