using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Application.Features.Problem.Commands.Delete;
using CodeSphere.Application.Features.Problem.Commands.SolveProblem;
using CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.ElasticSearchResponses;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{

    public class ProblemController : BaseController
    {

        private readonly IElasticSearchRepository _elasticSearchRepository;

        public ProblemController(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProblemAsync([FromBody]CreateProblemCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpPost("solve")]
        public async Task<ActionResult<Response>> SolveProblemAsync([FromForm] SubmitSolutionCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpGet("all-submissions")]
        public async Task<ActionResult<Response>> GetAllSubmissions([FromQuery]GetProblemSubmissionsQuery query)
         => ResponseResult(await mediator.Send(query));

        [HttpDelete("{problemId}")]
        public async Task<ActionResult<Response>> DeleteProblemAsync([FromRoute]int problemId)
                     => ResponseResult(await mediator.Send(new DeleteProblemCommand(problemId)));

        [HttpGet("{name}")]
        public async Task<IActionResult> GetProblemAsync([FromRoute]string name)
        {
            return Ok(await _elasticSearchRepository.SearchProblemsAsync(name));
        }
    }
}
