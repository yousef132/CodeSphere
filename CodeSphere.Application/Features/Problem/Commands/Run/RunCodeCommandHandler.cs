using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IExecutionService executionService;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor contextAccessor;
        private string UserId;

        public RunCodeCommandHandler(IUnitOfWork unitOfWork,
                                    IExecutionService executionService,
                                    IFileService fileService,
                                    IHttpContextAccessor contextAccessor)

        {
            this.unitOfWork = unitOfWork;
            this.executionService = executionService;
            this.fileService = fileService;
            this.contextAccessor = contextAccessor;
            var user = contextAccessor.HttpContext?.User;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<Response> Handle(RunCodeCommand request, CancellationToken cancellationToken)
        {
            var problem = await unitOfWork.ProblemRepository.GetProblemIncludingContestAndTestcases(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem Not Found");

            if (problem.Contest.ContestStatus == ContestStatus.Upcoming)
                return await Response.FailureAsync("Contest Not Started", System.Net.HttpStatusCode.Forbidden);

            if (problem.Contest.ContestStatus == ContestStatus.Running)
            {
                // return bad request if not registered 
                var isRegistered = await unitOfWork.UserContestRepository.IsRegistered(problem.ContestId, UserId);
                if (isRegistered == null)
                    return await Response.FailureAsync("You are not registered in this contest", System.Net.HttpStatusCode.Forbidden);
            }

            List<CustomTestcaseDto> customTestcases = null;
            customTestcases = System.Text.Json.JsonSerializer.Deserialize<List<CustomTestcaseDto>>(request.CustomTestcasesJson);

            string codeContent = await fileService.ReadFile(request.Code);

            var result = await executionService.ExecuteCodeAsync(
                         codeContent,
                         request.Language,
                         customTestcases,
                         problem.RunTimeLimit);

            return await Response.SuccessAsync(result, "Testcases run successfully !!");
        }
    }
}
