using CodeSphere.Application.Features.Submission.Queries.GetSubmissionData;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeSphere.WebApi.Controllers
{
    public class SubmissionController : BaseController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Response>> GetSubmissionData([FromRoute] int id)
            => ResponseResult(await mediator.Send(
                new GetSubmissionDataQuery
            {
                SubmissionId = id ,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            }));
    }
}
