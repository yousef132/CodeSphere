using CodeSphere.Application.Features.TestCase.Commands.Create;
using CodeSphere.Domain.Premitives;
using CodeSphere.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class TestcaseController : BaseController
    {
        private readonly ApplicationDbContext context;

        public TestcaseController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public async Task<ActionResult<Response>> CreateTestcaseAsync(CreateTestcaseCommand command)
        {
            return ResponseResult(await mediator.Send(command));
        }
        [HttpGet]
        public async Task<ActionResult> GetProblemTestcases(int problemId)
        {
            var testcases = context.Testcases.Select(t => t.Input).ToList();
            return Ok(testcases);

        }
    }
}
