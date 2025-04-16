using AutoMapper;
using CodeSphere.Application.Features.Contest.Command.Create;
using CodeSphere.Application.Features.Contest.Command.Update;
using CodeSphere.Application.Features.Contest.Queries.GetAllContests;
using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Mapping
{
    public class ContestProfile : Profile
    {
        public ContestProfile()
        {
            CreateMap<CreateContestCommand, Contest>();
            CreateMap<Tuple<Contest, bool>, GetAllContestsQueryResponse>()
                .ForMember(dest => dest.UserRegistered, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Item1.EndDate))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Item1.StartDate))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item1.Name))
                .ForMember(dest => dest.ContestStatus, opt => opt.MapFrom(src => src.Item1.ContestStatus))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id));

            CreateMap<UpdateContestCommand, Contest>();
        }
    }
}
