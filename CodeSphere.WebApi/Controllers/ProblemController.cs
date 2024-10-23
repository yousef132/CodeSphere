using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{

    public class ProblemController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Response>> CreateProblemAsync(CreateProblemCommand command)
         => ResponseResult(await mediator.Send(command));
    }
}
