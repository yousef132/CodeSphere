using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CodeSphere.Application.Features.Authentication.Queries.ResendConfirmEmail
{
	public class ResendConfirmEmailQueryHandler : IRequestHandler<ResendConfirmEmailQuery,Response>
	{
		private readonly IEmailService emailService;
		private readonly UserManager<ApplicationUser> userManager;

		public ResendConfirmEmailQueryHandler(IEmailService emailService,UserManager<ApplicationUser> userManager)
		{
			this.emailService = emailService;
			this.userManager = userManager;
		}

		public async Task<Response> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
		{
			var applicationUser = await userManager.FindByEmailAsync(request.Email);

			if (applicationUser is null)
				return await Response.FailureAsync("Email is Existed!", System.Net.HttpStatusCode.NotFound);

			if(applicationUser.EmailConfirmed)
				return await Response.FailureAsync("Email Already Confirmed!", System.Net.HttpStatusCode.NotFound);

			await emailService.SendConfirmationEmail(applicationUser);


			return await Response.SuccessAsync("Email Sent Successfully!");


		}
	}
}
