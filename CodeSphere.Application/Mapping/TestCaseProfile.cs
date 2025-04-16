using AutoMapper;
using CodeSphere.Application.Features.TestCase.Commands.Create;
using CodeSphere.Application.Features.Testcases.Commands.Update;
using CodeSphere.Application.Features.Testcases.Queries.GetTestCasesByProblemId;
using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Mapping
{
    public class TestCaseProfile : Profile
    {
        public TestCaseProfile()
        {
            CreateMap<GetTestCasesByProblemIdQuerey, Testcase>();
            CreateMap<CreateTestcaseCommand, Testcase>();
            CreateMap<UpdateTestcaseCommand, Testcase>()
                .ForMember(dest => dest.Output, opt => opt.MapFrom(src => src.ExpectedOutput))
                .ForMember(dest => dest.Input, opt => opt.MapFrom(src => src.Input));
        }
    }
}
