using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Application.Features.Problem.Commands.SolveProblem;
using CodeSphere.Application.Features.Problem.Queries.GetProblemTestCasesById;
using CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{

    public class ProblemController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Response>> CreateProblemAsync(CreateProblemCommand command)
         => ResponseResult(await mediator.Send(command));
        [HttpPost("solve")]
        public async Task<ActionResult<Response>> SolveProblemAsync([FromForm] SubmitSolutionCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpGet("{problemId}")]
        public async Task<ActionResult<Response>> GetProblemTestCasesByIdAsync([FromRoute] int problemId)
         => ResponseResult(await mediator.Send(new GetProblemTestCasesByIdQuery(problemId)));

        [HttpGet("all-submissions")]
        public async Task<ActionResult<Response>> GetAllSubmissions(GetProblemSubmissionsQuery query)
         => ResponseResult(await mediator.Send(query));


    }
}
