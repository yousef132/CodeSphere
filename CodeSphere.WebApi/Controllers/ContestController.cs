using CodeSphere.Application.Features.Contest.Command.Create;
using CodeSphere.Application.Features.Contest.Command.Register;
using CodeSphere.Application.Features.Contest.Queries;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{

    public class ContestController : BaseController
    {
        private readonly IResponseCacheService responseCacheService;

        public ContestController(IResponseCacheService responseCacheService)
        {
            this.responseCacheService = responseCacheService;
        }



        [HttpGet("{id}/problems")]
        //[Authorize(Roles = Roles.User)]
        public async Task<ActionResult<Response>> GetContestProblems([FromRoute] int id)
         => ResponseResult(await mediator.Send(new GetContestProblemsQuery(id)));


        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<Response>> CreateContest([FromBody] CreateContestCommand command)
           => ResponseResult(await mediator.Send(command));



        [HttpPost("register/{contestId}")]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult<Response>> RegisterInContest(int contestId)
           => ResponseResult(await mediator.Send(new RegisterInContestCommand(contestId)));






        [HttpPost("cache")]
        public async Task<ActionResult<Response>> cache(string key, string value)
        {
            await responseCacheService.CacheResponseAsync(key, value, TimeSpan.FromSeconds(6000));
            return Ok();

        }
        [HttpGet("get-cache")]
        public async Task<ActionResult<Response>> Getcache(string key)
        {
            var result = await responseCacheService.GetCachedResponseAsync(key);
            return Ok(result);
        }



    }


}
