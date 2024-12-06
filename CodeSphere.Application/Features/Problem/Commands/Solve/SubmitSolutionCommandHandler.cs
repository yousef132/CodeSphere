using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
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
        private readonly IFileService fileService;

        public SubmitSolutionCommandHandler(IProblemRepository problemRepository,
                                             IUnitOfWork unitOfWork,
                                             IMapper mapper,
                                             IExecutionService executionService,
                                             IFileService fileService)
        {
            this.problemRepository = problemRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.executionService = executionService;
            this.fileService = fileService;
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

            string codeContent = await fileService.ReadFile(request.Code);


            var result = await executionService.ExecuteCodeAsync(codeContent, request.Language, problemTestCases.ToList(), problem.RunTimeLimit, (decimal)problem.MemoryLimit);

            var baseSubmission = (result as BaseSubmissionResponse);
            var compilationError = (result as CompilationErrorResponse);
            var submission = new Submit
            {
                UserId = request.UserId,
                SubmissionDate = DateTime.UtcNow,
                ContestId = request.ContestId,
                Language = request.Language,
                Result = baseSubmission.SubmissionResult,
                Error = (result as CompilationErrorResponse)?.Message ?? "",
                ProblemId = request.ProblemId,
                SubmitTime = baseSubmission.ExecutionTime,
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
