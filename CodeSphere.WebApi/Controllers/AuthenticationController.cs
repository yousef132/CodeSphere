using CodeSphere.Application.Features.Authentication.Commands.Register;
using CodeSphere.Application.Features.Authentication.Queries.Login;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CodeSphere.WebApi.Controllers
{

    public class AuthenticationController : BaseController
	{
		[HttpPost("Register")]
		public async Task<ActionResult<Response>> Register(RegisterCommand command)
			 => ResponseResult(await mediator.Send(command));

		[HttpPost("Login")]
		public async Task<ActionResult<Response>> Login (LoginQuery query)
			=> ResponseResult(await mediator.Send(query));
	}
}
