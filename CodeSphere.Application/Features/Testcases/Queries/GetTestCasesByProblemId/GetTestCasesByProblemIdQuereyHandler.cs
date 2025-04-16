using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Premitives;
using MediatR;
using System.Net;

namespace CodeSphere.Application.Features.Testcases.Queries.GetTestCasesByProblemId
{
    public class GetTestCasesByProblemIdQuereyHandler : IRequestHandler<GetTestCasesByProblemIdQuerey, Response>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetTestCasesByProblemIdQuereyHandler(IProblemRepository problemRepository, IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(GetTestCasesByProblemIdQuerey request, CancellationToken cancellationToken)
        {
            var problem = await _unitOfWork.Repository<Domain.Models.Entities.Problem>()
                .GetByIdAsync(request.ProblemId);

            if (problem == null)
                return await Response.FailureAsync("Problem not found", HttpStatusCode.NotFound);

            var testCases = _problemRepository.GetTestCasesByProblemId(request.ProblemId);

            var formattedTestCases = testCases.Select(tc => new
            {
                tc.Id,
                Input = tc.Input.Replace("\r", "").Replace("\n", ""),
                tc.Output
            });

            var result = new
            {
                ProblemId = request.ProblemId,
                TestCases = formattedTestCases
            };

            return await Response.SuccessAsync(result, "TestCases fetched successfully", HttpStatusCode.OK);
        }
    }
}
