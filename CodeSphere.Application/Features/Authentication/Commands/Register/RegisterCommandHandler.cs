    using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CodeSphere.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      UserManager<ApplicationUser> userManager,
                                      IAuthService authService,
                                      IHttpContextAccessor httpContextAccessor,
                                      IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _authService = authService;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
        }
        public async Task<Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var applicationUser = await _userManager.FindByEmailAsync(request.Email);

            if (applicationUser is not null)
                return await Response.FailureAsync("Email is Existed!", System.Net.HttpStatusCode.BadRequest);


            var applicationUserByUserName = await _userManager.FindByNameAsync(request.UserName);

            if (applicationUserByUserName is not null)
                return await Response.FailureAsync("UserName is Existed!", System.Net.HttpStatusCode.BadRequest);

            var mappedUser = _mapper.Map<RegisterCommand, ApplicationUser>(request);

            var registerResult = await _userManager.CreateAsync(mappedUser, request.Password);
            if (!registerResult.Succeeded)
                return await Response.FailureAsync(
                            string.Join(", ", registerResult.Errors.Select(x => x.Description)),
                            System.Net.HttpStatusCode.BadRequest
                        );


            await emailService.SendConfirmationEmail(mappedUser);

            var registerCommandHandler = new RegisterCommandResponse()
            {
                Password = request.Password,
                Email = request.Email,
                Token = await _authService.CreateTokenAsync(mappedUser, _userManager)
            };

            return await Response.SuccessAsync(registerCommandHandler, "Registered Successfully", System.Net.HttpStatusCode.Created);
        }


    }
}
