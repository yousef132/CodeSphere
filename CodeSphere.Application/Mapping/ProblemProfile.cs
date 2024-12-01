using AutoMapper;
using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Application.Features.Problem.Queries.GetAll;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Responses.ElasticSearchResponses;

namespace CodeSphere.Application.Mapping
{
    public class ProblemProfile : Profile
    {
        public ProblemProfile()
        {
            CreateMap<CreateProblemCommand, Problem>();
            CreateMap<Problem, CreateProblemCommandResponse>();
            CreateMap<ProblemDocument, GetAllQueryResponse>();


        }
    }
}
