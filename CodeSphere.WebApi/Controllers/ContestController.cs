using CodeSphere.Application.Features.Contest.Command.Create;
using CodeSphere.Application.Features.Contest.Command.Delete;
using CodeSphere.Application.Features.Contest.Command.Register;
using CodeSphere.Application.Features.Contest.Command.Update;
using CodeSphere.Application.Features.Contest.Queries.GetAllContests;
using CodeSphere.Application.Features.Contest.Queries.GetContestProblems;
using CodeSphere.Application.Features.Contest.Queries.GetContestStanding;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.Contest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class ContestController : BaseController
    {
        private readonly ICacheService responseCacheService;

        public ContestController(ICacheService responseCacheService)
        {
            this.responseCacheService = responseCacheService;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<Response>> CreateContest([FromBody] CreateContestCommand command)
           => ResponseResult(await mediator.Send(command));

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<Response>> UpdateContest(int id, [FromBody] UpdateContestCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Id mismatch");
            }
            return ResponseResult(await mediator.Send(command));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<Response>> DeleteContest(int id)
        {
            return ResponseResult(await mediator.Send(new DeleteContestCommand { Id = id }));
        }

        [HttpGet("{id}/problems")]
        [ProducesResponseType(typeof(ContestProblemResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<Response>> GetContestProblems([FromRoute] int id)
        => ResponseResult(await mediator.Send(new GetContestProblemsQuery(id)));

        [HttpPost("register/{contestId}")]
        [Authorize]
        public async Task<ActionResult<Response>> RegisterInContest(int contestId)
           => ResponseResult(await mediator.Send(new RegisterInContestCommand(contestId)));

        [HttpGet]
        public async Task<ActionResult<Response>> GetAllContests()
          => ResponseResult(await mediator.Send(new GetAllContestsQuery()));

        //[HttpPost("cache")]
        //public async Task<ActionResult<Response>> cache(string key, string value)
        //{
        //    await responseCacheService.CacheResponseAsync(key, value, TimeSpan.FromSeconds(6000));
        //    return Ok();

        //}

        //[HttpGet("get-cache")]
        //public async Task<ActionResult<Response>> Getcache(string key)
        //{
        //    var result = await responseCacheService.GetCachedResponseAsync(key);
        //    return Ok(result);
        //}

        [HttpGet("{contestId}/standing")]
        public async Task<ActionResult<Response>> GetContestStanding(int contestId)
         => ResponseResult(await mediator.Send(new GetContestStandingQuery(contestId)));





    }
}
