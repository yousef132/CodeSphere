using CodeSphere.Application.Features.Authentication.Commands.Register;
using CodeSphere.Application.Features.Authentication.Queries.ConfirmEmail;
using CodeSphere.Application.Features.Authentication.Queries.Login;
using CodeSphere.Application.Features.Authentication.Queries.ResendConfirmEmail;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{

    public class AuthenticationController : BaseController
    {
        [HttpPost("Register")]
        public async Task<ActionResult<Response>> Register(RegisterCommand command)
             => ResponseResult(await mediator.Send(command));

        [HttpPost("Login")]
        public async Task<ActionResult<Response>> Login(LoginQuery query)
            => ResponseResult(await mediator.Send(query));


		[HttpGet("confirm-email")]
		public async Task<ActionResult<Response>> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
			=> ResponseResult(await mediator.Send(query));




		[HttpGet("resend-confirm-email")]
		public async Task<ActionResult<Response>> ResendConfirmEmail([FromQuery] ResendConfirmEmailQuery query)
			=> ResponseResult(await mediator.Send(query));
	}
}
