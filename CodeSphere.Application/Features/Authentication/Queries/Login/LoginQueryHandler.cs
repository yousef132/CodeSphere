using AutoMapper;
using CodeSphere.Application.Features.Authentication.Commands.Register;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Queries.Login
{
	public class LoginQueryHandler : IRequestHandler<LoginQuery, Response>
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public LoginQueryHandler( UserManager<ApplicationUser> userManager,
								  SignInManager<ApplicationUser> signInManager,
                                  IMapper mapper,
								  IAuthService authService
								)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;
			_authService = authService;
		}
        public async Task<Response> Handle(LoginQuery request, CancellationToken cancellationToken)
		{
		    var mappedUser = _mapper.Map<LoginQuery, ApplicationUser>(request);

		    var applicationUser = await _userManager.FindByEmailAsync(request.Email);
			if (applicationUser is null)
				return await Response.FailureAsync("Invalid Login", System.Net.HttpStatusCode.Unauthorized);

			var resultLogin = await _signInManager.CheckPasswordSignInAsync(applicationUser, request.Password, false);
			if (!resultLogin.Succeeded)
				return await Response.FailureAsync("You Are Unauthorized");

			var loginQueryResponse = new LoginQueryResponse()
			{
				Password = request.Password,
				Email = request.Email,
				Token = await _authService.CreateTokenAsync(applicationUser, _userManager)
			};

			return await Response.SuccessAsync(loginQueryResponse, "Logined Successfully");
		}
	}
}
