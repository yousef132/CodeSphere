using CodeSphere.Application.Features.Plagiarism.Queries.GetByContestIdQuery;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class PlagiarismController : BaseController
    {
        private readonly IPlagiarismService plagiarismService;
        public PlagiarismController(IPlagiarismService plagiarismService)
        {
            this.plagiarismService = plagiarismService;
        }

        [HttpPost("test")]
        public IActionResult GetSimilarity([FromBody] CodeSimilarityRequest request)
        {
            return Ok(plagiarismService.GetSimilarity(request.Code1, request.Code2));
        }

        [HttpPost]
        public async Task<ActionResult<Response>> GetSimilarityCases(GetByContestIdQuery request)
            => ResponseResult(await mediator.Send(request));
    }
}
