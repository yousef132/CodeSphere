using CodeSphere.Application.Helpers;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IHttpContextAccessor httpContextAccessor;

		public EmailService(IOptions<EmailSettings> emailSettings,
                                    UserManager<ApplicationUser> userManager,
                                    IHttpContextAccessor httpContextAccessor)
        {
			this._emailSettings = emailSettings.Value;
			this.userManager = userManager;
			this.httpContextAccessor = httpContextAccessor;
		}

		public async Task SendConfirmationEmail(ApplicationUser user)
		{

			var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


			var origin = httpContextAccessor.HttpContext?.Request.Headers["Host"];
			string link = $"https://{origin}/api/Authentication/confirm-email?userId={user.Id}&code={code}";
			await SendEmailAsync(user.Email, link, "Confirming Email");
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
