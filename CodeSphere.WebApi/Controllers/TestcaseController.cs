using CodeSphere.Application.Features.TestCase.Commands.Create;
using CodeSphere.Application.Features.Testcases.Commands.Delete;
using CodeSphere.Application.Features.Testcases.Commands.Update;
using CodeSphere.Application.Features.Testcases.Queries.GetTestCasesByProblemId;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class TestcaseController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Response>> CreateTestcaseAsync(CreateTestcaseCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpGet("{problemId}")]
        public async Task<ActionResult<Response>> GetProblemTestCasesByIdAsync([FromRoute] int problemId)
         => ResponseResult(await mediator.Send(new GetTestCasesByProblemIdQuerey(problemId)));

        [HttpDelete("{testcaseId}")]
        public async Task<ActionResult<Response>> DeleteTestcaseAsync([FromRoute] int testcaseId)
         => ResponseResult(await mediator.Send(new DeleteTestcaseCommand(testcaseId)));


        [HttpPut("{testcaseId}")]
        public async Task<ActionResult<Response>> UpdateTestcaseAsync([FromRoute] int testcaseId, [FromBody] UpdateTestcaseCommand command)
        {
            if (testcaseId != command.TestcaseId)
                return BadRequest("TestcaseId in the route and body must match.");

            return ResponseResult(await mediator.Send(command));
        }
    }
}



