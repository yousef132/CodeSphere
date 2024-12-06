using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
	public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Response>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetByIdQueryHandler(IUnitOfWork unitOfWork,
			IMapper mapper)
        {
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
        public async Task<Response> Handle(GetByIdQuery request, CancellationToken cancellationToken)
		{
			// Get the Problem Details from Repository
			var getSpecificProblem = await _unitOfWork.ProblemRepository
			.GetProblemDetailsAsync(request.ProblemId, cancellationToken);


			// Check if Problem was not Found
			if (getSpecificProblem is null)
				return await Response.FailureAsync("Problem not Found", HttpStatusCode.NotFound);
		
			
			getSpecificProblem.Testcases = getSpecificProblem.Testcases?.Take(3).ToList() ?? [];

			// Map to the response 
			var response = _mapper.Map<GetByIdQueryResponse>(getSpecificProblem);

			// Populate Accepted and Submissions counts
			response.Accepted = _unitOfWork.ProblemRepository.GetAcceptedProblemCount(request.ProblemId);

			response.Submissions = _unitOfWork.ProblemRepository.GetSubmissionsProblemCount(request.ProblemId);

			// Check if the user has solved the problem
			response.IsSolved = _unitOfWork.ProblemRepository.CheckUserSolvedProblem(
				request.ProblemId, request.UserId, cancellationToken);

			// Return the success response
			return await Response.SuccessAsync(response, "Problem Found", HttpStatusCode.OK);	
		}
	}
}
