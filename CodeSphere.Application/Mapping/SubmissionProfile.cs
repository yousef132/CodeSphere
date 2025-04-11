using AutoMapper;
using CodeSphere.Application.Features.Contest.Queries.GetContestSubmissionsHistory;
using CodeSphere.Application.Features.Submission.Queries.GetProblemSubmissions;
using CodeSphere.Application.Features.Submission.Queries.GetSubmissionData;
using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Mapping
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<Submit, GetSubmissionDataQueryResponse>();
            CreateMap<Submit, GetProblemSubmissionsResponse>()
                .ForMember(dest => dest.SubmitTime, opt => opt.MapFrom(src => src.SubmitTime))
                .ForMember(dest => dest.SubmitMemory, opt => opt.MapFrom(src => src.SubmitMemory))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));


            CreateMap<Submit, GetContestSubmissionsQueryResponse>()
                .ForMember(dest => dest.ProblemName, opt => opt.MapFrom(src => src.Problem.Name))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.SubmitTime))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language));
        }
    }
}
