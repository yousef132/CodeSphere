using AutoMapper;
using CodeSphere.Application.Features.TestCase.Commands.Create;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.TestCases.Commands.Create
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
            var problem = await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem not found!", System.Net.HttpStatusCode.NotFound);

            var newTestcase = _mapper.Map<CodeSphere.Domain.Models.Entities.Testcase>(request);

            await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().AddAsync(newTestcase);
            await _unitOfWork.CompleteAsync();

            return await Response.SuccessAsync(newTestcase, "Test case added successfully.", System.Net.HttpStatusCode.Created);
        }
    }
}
