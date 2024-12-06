using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllQueryHandler : IRequestHandler<GetAllProblemsQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response> Handle(GetAllProblemsQuery request, CancellationToken cancellationToken)
        {
            var problems = await _unitOfWork.ElasticSearchRepository.SearchProblemsAsync(request.ProblemName, request.TopicsIds, request.Difficulty);

            if (problems.IsNullOrEmpty())
                return await Response.FailureAsync("No Problems Found", HttpStatusCode.NotFound);

            var responses = new List<GetAllQueryResponse>();
            var submissions = await _unitOfWork.Repository<Submit>().GetAllAsync();

            foreach (var problem in problems)
            {
                var submission = submissions.Where(s => s.ProblemId == problem.Id && s.Result == SubmissionResult.Accepted && s.UserId == request.UserId);
                var mappedSubmission = _mapper.Map<GetAllQueryResponse>(problem);
                mappedSubmission.IsSolved = !submission.IsNullOrEmpty();
                responses.Add(mappedSubmission);
            }

            return await Response.SuccessAsync(responses, "Problems Found", HttpStatusCode.OK);
        }

    }
}
