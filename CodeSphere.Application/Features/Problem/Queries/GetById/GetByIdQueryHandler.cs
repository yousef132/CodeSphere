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

			var response = _mapper.Map<GetByIdQueryResponse>(getSpecificProblem);

			response.Accepted = _unitOfWork.ProblemRepository.GetAcceptedProblemCount(request.ProblemId);

			response.Submissions = _unitOfWork.ProblemRepository.GetSubmissionsProblemCount(request.ProblemId);

			return await Response.SuccessAsync(response, "Problem Found", HttpStatusCode.OK);	
		}
	}
}
