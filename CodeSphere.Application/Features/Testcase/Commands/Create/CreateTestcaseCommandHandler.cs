using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.TestCase.Commands.Create
{
    public class CreateTestcaseCommandHandler : IRequestHandler<CreateTestcaseCommand, Response>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTestcaseCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(CreateTestcaseCommand request, CancellationToken cancellationToken)
        {
            // Fully qualify the Problem entity to avoid ambiguity
            var problem = await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
            {
                return await Response.FailureAsync("Problem not found.");
            }

            // Fully qualify the Testcase entity to avoid ambiguity
            var newTestcase = new CodeSphere.Domain.Models.Entities.Testcase
            {
                ProblemId = request.ProblemId,
                Input = request.Input,
                Output = request.Output
            };

            // Save the Testcase
            await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().AddAsync(newTestcase);
            await _unitOfWork.CompleteAsync();

            return await Response.SuccessAsync(newTestcase, "Test case added successfully.");
        }
    }
}
