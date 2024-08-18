using Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Externals
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message, string clientName)
        {
            IConfigurationSection smtpSettings = _configuration.GetSection("SmtpSettings");
            SmtpClient smtpClient = new()
            {
                Host = smtpSettings["Host"]!,
                Port = int.Parse(smtpSettings["Port"]!),
                EnableSsl = Convert.ToBoolean(smtpSettings["EnableSsl"]!),
                Credentials = new NetworkCredential()
                {
                    UserName = Environment.GetEnvironmentVariable("AUTHSYS_DEV_EMAIL"),
                    Password = Environment.GetEnvironmentVariable("AUTHSYS_DEV_PASSWORD")
                }
            };

            MailMessage mailMessage = new()
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("AUTHSYS_DEV_EMAIL")!, clientName),
                Subject = subject,
                Body = message
            };

            mailMessage.To.Add(toEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch
            {
                throw new Exception("Email failed to send");
            }
        }
    }
}
