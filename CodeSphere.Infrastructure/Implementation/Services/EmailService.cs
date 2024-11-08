using CodeSphere.Application.Helpers;
using CodeSphere.Domain.Abstractions.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this._emailSettings = emailSettings.Value;
        }
        public async Task<bool> SendEmailAsync(string email, string _message, string? reason)
        {
            try
            {
                using (var client = new SmtpClient())
                {

                    await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                    client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $"{_message}",
                        TextBody = "welcome",
                    };
                    var message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    message.From.Add(new MailboxAddress("Code Sphere", _emailSettings.FromEmail));
                    message.To.Add(new MailboxAddress("Dev", email));
                    message.Subject = reason == null ? "" : reason;
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
