using AutoMapper;
using CodeSphere.Application.Features.Contest.Command.Create;
using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Mapping
{
    public class ContestProfile : Profile
    {
        public ContestProfile()
        {
            CreateMap<CreateContestCommand, Contest>();
        }
    }
}
