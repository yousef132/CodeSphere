using CodeSphere.Application.Features.TestCase.Commands.Create;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.DTOs;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.TestCases.Commands.Create
{
    public class CreateTestcaseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTestcaseCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Response> Handle(CreateTestcaseCommand request, CancellationToken cancellationToken)
        {
            var problem = await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem not found!", System.Net.HttpStatusCode.NotFound);

            var newTestcase = new Domain.Models.Entities.Testcase
            {
                ProblemId = request.ProblemId,
                Input = request.Input,
                Output = request.expectedOutput
            };

            await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().AddAsync(newTestcase);
            await _unitOfWork.CompleteAsync();

            var responseDto = new TestcaseResponseDTO
            {
                Id = newTestcase.Id,
                Input = newTestcase.Input,
                Output = newTestcase.Output
            };

            return await Response.SuccessAsync(responseDto, "Test case added successfully.", System.Net.HttpStatusCode.Created);
        }
    }
}
