using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Premitives;
using MediatR;
using System.Net;

namespace CodeSphere.Application.Features.Testcases.Commands.Update
{
    public class UpdateTestcaseCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IProblemRepository problemRepository) : IRequestHandler<UpdateTestcaseCommand, Response>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IProblemRepository _problemRepository = problemRepository;

        public async Task<Response> Handle(UpdateTestcaseCommand request, CancellationToken cancellationToken)
        {
            var existingTestcase = await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().GetByIdAsync(request.TestcaseId);
            if (existingTestcase == null)
                return await Response.FailureAsync("Testcase not found!", System.Net.HttpStatusCode.NotFound);

            _mapper.Map(request, existingTestcase);

            await _unitOfWork.Repository<CodeSphere.Domain.Models.Entities.Testcase>().UpdateAsync(existingTestcase);
            await _unitOfWork.CompleteAsync();

            var testCases = _problemRepository.GetTestCasesByProblemId(existingTestcase.ProblemId);

            var formattedTestCases = testCases.Select(tc => new
            {
                tc.Id,
                Input = tc.Input.Replace("\r", "").Replace("\n", ""),
                tc.Output
            });

            var result = new
            {
                existingTestcase.ProblemId,
                TestCases = formattedTestCases
            };

            return await Response.SuccessAsync(result, "Testcases updated successfully", HttpStatusCode.OK);
        }
    }
}
