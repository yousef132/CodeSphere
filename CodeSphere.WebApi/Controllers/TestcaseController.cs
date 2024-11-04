using Azure;
using CodeSphere.Application.Features.TestCase.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class TestcaseController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<Response>> CreateTestcaseAsync(CreateTestcaseCommand command)
        {
            return ResponseResult(await mediator.Send(command));
        }
    }
}
