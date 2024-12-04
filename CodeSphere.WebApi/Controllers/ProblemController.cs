﻿using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Application.Features.Problem.Commands.Delete;
using CodeSphere.Application.Features.Problem.Commands.SolveProblem;
using CodeSphere.Application.Features.Problem.Queries.GetAll;
using CodeSphere.Application.Features.Problem.Queries.GetById;
using CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.ElasticSearchResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeSphere.WebApi.Controllers
{

    public class ProblemController : BaseController
    {

        private readonly IElasticSearchRepository _elasticSearchRepository;

        public ProblemController(IElasticSearchRepository elasticSearchRepository)
        {
            _elasticSearchRepository = elasticSearchRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProblemAsync([FromBody]CreateProblemCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpPost("solve")]
        public async Task<ActionResult<Response>> SolveProblemAsync([FromForm] SubmitSolutionCommand command)
         => ResponseResult(await mediator.Send(command));

        [HttpGet("all-submissions")]
        public async Task<ActionResult<Response>> GetAllSubmissions([FromQuery]GetProblemSubmissionsQuery query)
         => ResponseResult(await mediator.Send(query));

        [HttpDelete("{problemId}")]
        public async Task<ActionResult<Response>> DeleteProblemAsync([FromRoute]int problemId)
                     => ResponseResult(await mediator.Send(new DeleteProblemCommand(problemId)));

        [Authorize]
        [HttpPost("problems")]
        public async Task<ActionResult<Response>> GetProblemsAsync([FromBody]GetAllProblemsQuery query)
        {
            query.UserId = GetCurrentUserId();
            return ResponseResult(await mediator.Send(query));
        }


        //[HttpGet("{name}")]
        //public async Task<IActionResult> GetProblemAsync([FromRoute]string name)
        //{
        //    return Ok(await _elasticSearchRepository.SearchProblemsAsync(name));
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProblemDetails([FromRoute] int id)
            => ResponseResult(await mediator.Send(new GetByIdQuery (id)));
	}
}
