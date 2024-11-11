using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Responses.SubmissionResponses;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Commands.SolveProblem
{
    public class SubmitSolutionCommandHandler : IRequestHandler<SubmitSolutionCommand, Response>
    {
        private readonly IProblemRepository problemRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IExecutionService executionService;

        public SubmitSolutionCommandHandler(IProblemRepository problemRepository,
                                            IUnitOfWork unitOfWork,
                                            IMapper mapper,
                                            IExecutionService executionService)
        {
            this.problemRepository = problemRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.executionService = executionService;
        }
        public async Task<Response> Handle(SubmitSolutionCommand request, CancellationToken cancellationToken)
        {
            var problem = await unitOfWork.Repository<Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem Not Found", System.Net.HttpStatusCode.NotFound);

            var contest = await unitOfWork.Repository<Contest>().GetByIdAsync(request.ContestId);

            if (contest == null)
                return await Response.FailureAsync("Contest Not Found", System.Net.HttpStatusCode.NotFound);

            var problemTestCases = problemRepository.GetTestCasesByProblemId(request.ProblemId);

            string codeContent;
            using (var reader = new StreamReader(request.Code.OpenReadStream()))
            {
                codeContent = await reader.ReadToEndAsync();
            }

            var result = await executionService.ExecuteCodeAsync(codeContent, request.Language, problemTestCases.ToList(), problem.RunTimeLimit);

            var submission = new Submit
            {
                UserId = request.UserId,
                SubmissionDate = DateTime.UtcNow,
                ContestId = request.ContestId,
                Language = request.Language,
                Result = (result as BaseSubmissionResponse).SubmissionResult,
                Error = (result as CompilationErrorResponse)?.Message ?? "",
                ProblemId = request.ProblemId,
                SubmitTime = (result as BaseSubmissionResponse).ExecutionTime,
                Code = codeContent,
                SubmitMemory = 1m
            };

            await unitOfWork.Repository<Submit>().AddAsync(submission);
            await unitOfWork.CompleteAsync();

            // save submission result in database
            return await Response.SuccessAsync(result, "Submitted Successfully", System.Net.HttpStatusCode.Created);
        }
    }
}
