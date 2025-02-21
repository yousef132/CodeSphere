using CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions;
using CodeSphere.Application.Features.Submission.Queries.GetSubmissionData;
using CodeSphere.Domain.Premitives;
using Elastic.Clients.Elasticsearch.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace CodeSphere.WebApi.Controllers
{
   
    public class SubmissionsController : BaseController
    {
        [HttpGet("{submissionId}")]
        [Authorize]
        public async Task<ActionResult<Response>> GetSubmissionData([FromRoute] int submissionId)
            => ResponseResult(await mediator.Send(new GetSubmissionDataQuery(submissionId)));

        [HttpGet("Problem/{problemId}")]
        public async Task<ActionResult<Response>> GetAllSubmissions([FromRoute] int problemId, [FromQuery] string UserId)
         => ResponseResult(await mediator.Send(new GetProblemSubmissionsQuery(problemId, UserId)));
    }
}
