using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Application.Features.Problem.Commands.Delete;
using CodeSphere.Application.Features.Problem.Commands.Run;
using CodeSphere.Application.Features.Problem.Commands.SolveProblem;
using CodeSphere.Application.Features.Problem.Queries.GetAll;
using CodeSphere.Application.Features.Problem.Queries.GetById;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    [Route("Problems")]
    public class ProblemController : BaseController
    {

        private readonly IElasticSearchRepository _elasticSearchRepository;

        public ProblemController(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]

        public async Task<ActionResult<Response>> CreateProblemAsync([FromBody] CreateProblemCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpPost("solve")]
        [RateLimitingFilter(5)]
        [Authorize]

        public async Task<ActionResult<Response>> SolveProblemAsync([FromForm] SubmitSolutionCommand command)
         => ResponseResult(await mediator.Send(command));



        [HttpPost("run")]
        [RateLimitingFilter(5)]
        public async Task<ActionResult<Response>> RunProblemTestcasesAsync([FromForm] RunCodeCommand command)
         => ResponseResult(await mediator.Send(command));



        [HttpDelete("{problemId}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<Response>> DeleteProblemAsync([FromRoute] int problemId)
                     => ResponseResult(await mediator.Send(new DeleteProblemCommand(problemId)));

        [HttpGet]
        public async Task<ActionResult<Response>> GetProblemsAsync(
            [FromQuery] List<string>? TopicsNames,
            [FromQuery] string? problemName,
            [FromQuery] Difficulty? difficulty,
            [FromQuery] Domain.Models.Entities.Status? status,
            [FromQuery] SortBy? sortBy,
            [FromQuery] Order? order,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var query = new GetAllProblemsQuery(null,
                TopicsNames,
                problemName,
                difficulty,
                pageNumber,
                pageSize,
                status,
                sortBy,
                order);
            return ResponseResult(await mediator.Send(query));
        }



        //[HttpGet("{name}")]
        //public async Task<IActionResult> GetProblemAsync([FromRoute]string name)
        //{
        //    return Ok(await _elasticSearchRepository.SearchProblemsAsync(name));
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProblemDetails([FromRoute] int id)
            => ResponseResult(await mediator.Send(new GetByIdQuery(id)));
    }
}
