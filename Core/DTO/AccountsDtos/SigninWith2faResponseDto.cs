namespace Core.DTO.AccountsDtos
{
    public class SigninWith2faResponseDto : SigninResponseDto
    {
        public string? TwoFactorsAuthenticationCode { get; set; }
        public string? UserEmail { get; set; }
    }
}
