using AutoMapper;
using CodeSphere.Application.Features.Problem.Commands.Create;
using CodeSphere.Application.Features.Problem.Queries.GetAll;
using CodeSphere.Application.Features.Problem.Queries.GetById;
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
            CreateMap<Problem, GetByIdQueryResponse>()
                .ForMember(d => d.TestCases, O => O.MapFrom(S => S.Testcases))
                .ForMember(d => d.Topics, O => O.MapFrom(S => S.ProblemTopics));


            CreateMap<ProblemTopic,TopicDto>()
				.ForMember(d => d.Id, O => O.MapFrom(S => S.Topic.Id))
				.ForMember(d => d.Name, O => O.MapFrom(S => S.Topic.Name));




            CreateMap<Testcase, TestCasesDto>()
                .ForMember(d => d.ExpectedOutput, O => O.MapFrom(S => S.Output));








        }
    }
}
