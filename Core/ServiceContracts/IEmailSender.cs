namespace Core.ServiceContracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message, string clientName);
    }
}
