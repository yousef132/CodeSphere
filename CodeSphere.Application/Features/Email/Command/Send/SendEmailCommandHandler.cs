using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CodeSphere.Application.Features.Email.Command.Send
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, Response>
    {
        private readonly IEmailService emailService;
        private readonly UserManager<ApplicationUser> userManager;

        public SendEmailCommandHandler(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            this.emailService = emailService;
            this.userManager = userManager;
        }
        public async Task<Response> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return await Response.FailureAsync("Email Not Found", System.Net.HttpStatusCode.NotFound);

            var result = await emailService.SendEmailAsync(request.Email, request.Message, null);
            if (result)
                return await Response.SuccessAsync(result, "Email Sent Successfully");

            return await Response.FailureAsync("Error While Sending Email", System.Net.HttpStatusCode.ServiceUnavailable);
        }
    }
}
