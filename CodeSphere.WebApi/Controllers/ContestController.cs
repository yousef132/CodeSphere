using CodeSphere.Application.Features.Contest.Queries;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetContestProblems(int id)
         => ResponseResult(await mediator.Send(new GetContestProblemsQuery(id)));


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
