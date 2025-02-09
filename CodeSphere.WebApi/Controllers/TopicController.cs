using CodeSphere.Application.Features.Topic.Commands;
using CodeSphere.Application.Features.Topic.Queries.GetAll;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.WebApi.Controllers
{
    [Route("Topics")]
    public class TopicController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<Response>> GetTopics() 
            => Ok(await mediator.Send(new GetAllTopicsQuery()));

        [HttpPost]
        public async Task<ActionResult<Response>> CreateTopic([FromBody] CreateTopicCommand command)
            => Ok(await mediator.Send(command));
    }
}
