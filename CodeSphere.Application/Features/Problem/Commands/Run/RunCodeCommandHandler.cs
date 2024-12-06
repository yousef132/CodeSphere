using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, Response>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IExecutionService executionService;
        private readonly IFileService fileService;

        public RunCodeCommandHandler(IUnitOfWork unitOfWork,
                                    IExecutionService executionService,
                                    IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.executionService = executionService;
            this.fileService = fileService;
        }
        public async Task<Response> Handle(RunCodeCommand request, CancellationToken cancellationToken)
        {
            var problem = await unitOfWork.Repository<Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem Not Found");
            List<CustomTestcaseDto> customTestcases = null;
            customTestcases = System.Text.Json.JsonSerializer.Deserialize<List<CustomTestcaseDto>>(request.CustomTestcasesJson);

            string codeContent = await fileService.ReadFile(request.Code);

            var result = await executionService.ExecuteCodeAsync(
                         codeContent,
                         request.Language,
                         customTestcases,
                         problem.RunTimeLimit,
                         (int)problem.MemoryLimit);

            return await Response.SuccessAsync(result, "Testcases run successfully !!");
        }
    }
    public class RunCodeCommandResponse
    {
        public RunCodeCommandResponse(string input, string output, bool passed)
        {
            Input = input;
            Output = output;
            Passed = passed;
        }

        public string Input { get; set; }
        public string Output { get; set; }
        public bool Passed { get; set; }

    }
}
