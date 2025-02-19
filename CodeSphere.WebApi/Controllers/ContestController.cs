using CodeSphere.Application.Features.Contest.Queries;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{

    public class ContestController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetContestProblems(int id)
         => ResponseResult(await mediator.Send(new GetContestProblemsQuery(id)));
    }
}
