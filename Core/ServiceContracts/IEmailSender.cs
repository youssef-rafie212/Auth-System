namespace Core.ServiceContracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string fromEmail, string toEmail, string subject, string message);
    }
}
