using AutoMapper;
using CodeSphere.Application.Features.Problem.Queries.GetAll;
using CodeSphere.Domain.Abstractions;
using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Responses.ElasticSearchResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Mapping.Resolvers
{
    public class TopicResolver : IValueResolver<ProblemDocument, GetAllQueryResponse, List<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<string> Resolve(ProblemDocument source, GetAllQueryResponse destination, List<string> destMember, ResolutionContext context)
        {
            return _unitOfWork.TopicRepository
                .GetTopicNamesByIdsAsync(source.Topics).Result;  // This is a blocking call :( 
        }
    }

}
