using CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions;
using CodeSphere.Application.Features.Submission.Queries.GetSubmissionData;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    [Authorize]
    [Route("Submissions")]
    public class SubmissionController : BaseController
    {
        [HttpGet("{id}")]

        public async Task<ActionResult<Response>> GetSubmissionData([FromRoute] int id)
            => ResponseResult(await mediator.Send(new GetSubmissionDataQuery(id)));

        [HttpGet("Problem/{id}")]

        public async Task<ActionResult<Response>> GetAllSubmissions([FromRoute] int id)
         => ResponseResult(await mediator.Send(new GetProblemSubmissionsQuery(id)));
    }
}
