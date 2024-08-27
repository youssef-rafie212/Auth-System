namespace Core.DTO.AccountsDtos
{
    public class SigninWithout2faResponseDto : SigninResponseDto
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
