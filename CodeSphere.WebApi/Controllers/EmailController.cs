using CodeSphere.Application.Features.Email.Command.Send;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    public class EmailController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
          => ResponseResult(await mediator.Send(command));
    }
}
