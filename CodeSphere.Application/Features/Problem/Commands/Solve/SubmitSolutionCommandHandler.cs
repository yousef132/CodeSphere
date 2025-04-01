using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.SubmissionResponses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Problem.Commands.SolveProblem
{
    public class SubmitSolutionCommandHandler : IRequestHandler<SubmitSolutionCommand, Response>
    {
        private readonly IProblemRepository problemRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IExecutionService executionService;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ICacheService cacheService;
        private string UserId;

        public SubmitSolutionCommandHandler(IProblemRepository problemRepository,
                                             IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                             IExecutionService executionService,
                                             IFileService fileService,
                                             IHttpContextAccessor contextAccessor,
                                             ICacheService cacheService)
        {
            this.problemRepository = problemRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.executionService = executionService;
            this.fileService = fileService;
            this.contextAccessor = contextAccessor;
            this.cacheService = cacheService;
            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(SubmitSolutionCommand request, CancellationToken cancellationToken)
        {
            var problem = await unitOfWork.ProblemRepository.GetProblemIncludingContestAndTestcases(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem Not Found", System.Net.HttpStatusCode.NotFound);

            if (problem.Contest == null)
                return await Response.FailureAsync("Contest Not Found", System.Net.HttpStatusCode.NotFound);

            string codeContent = await fileService.ReadFile(request.Code);

            var result = await executionService.ExecuteCodeAsync(codeContent, request.Language, problem.Testcases.ToList(), problem.RunTimeLimit);

            var baseSubmissionResponse = (result as BaseSubmissionResponse);
            var acceptedSubmission = (result as AcceptedResponse);
            var compilationError = (result as CompilationErrorResponse);
            var submission = new Submit
            {
                UserId = UserId,
                SubmissionDate = DateTime.UtcNow,
                ContestId = request.ContestId,
                Language = request.Language,
                Result = baseSubmissionResponse.SubmissionResult,
                Error = (result as CompilationErrorResponse)?.Message ?? "",
                ProblemId = request.ProblemId,
                SubmitTime = acceptedSubmission?.ExecutionTime ?? null,
                Code = codeContent,
                SubmitMemory = 0m
            };


            if (problem.Contest.ContestStatus == ContestStatus.Running)
            {
                // while the contest  : 

                // update the sorted set and add the submission for the cache as (submittionId, submissionDate, result)
                // always add the submission to the user hash
                // for the sorted set : 
                // if the user has already submitted the problem (accepted) then don't update the global sorted set
                // if the user has not submitted the problem then update the global sorted set
                // the key for the hash : leaderboard:submission:{submissionId}

                cacheService.CacheContestStanding()
            }
            // insert the result in the database 

            await unitOfWork.Repository<Submit>().AddAsync(submission);
            await unitOfWork.CompleteAsync();

            // save submission result in database
            return await Response.SuccessAsync(result, "Submitted Successfully", System.Net.HttpStatusCode.Created);
        }
    }
}
