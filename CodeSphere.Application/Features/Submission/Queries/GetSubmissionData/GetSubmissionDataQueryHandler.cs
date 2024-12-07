using AutoMapper;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Submission.Queries.GetSubmissionData
{
    public class GetSubmissionDataQueryHandler : IRequestHandler<GetSubmissionDataQuery, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubmissionDataQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(GetSubmissionDataQuery request, CancellationToken cancellationToken)
        {
            var sub = await _unitOfWork.Repository<Submit>().GetByIdAsync(request.SubmissionId);
            if (sub == null)
            {
                return await Response.FailureAsync("Submission not found" , System.Net.HttpStatusCode.NotFound);
            }
            if(sub.UserId != request.UserId)
            {
                return await Response.FailureAsync("You are not authorized to view this submission", System.Net.HttpStatusCode.Unauthorized);
            }
            
            var mappedSub = _mapper.Map<GetSubmissionDataQueryResponse>(sub);
            return await Response.SuccessAsync(mappedSub, "Submission fetched successfully", System.Net.HttpStatusCode.OK);
        }
    }
}
