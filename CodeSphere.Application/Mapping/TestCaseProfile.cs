using AutoMapper;
using CodeSphere.Application.Features.Problem.Queries.GetProblemTestCasesById;
using CodeSphere.Application.Features.TestCase.Commands.Create;
using CodeSphere.Domain.Models.Entities;

namespace CodeSphere.Application.Mapping
{
    public class TestCaseProfile : Profile
    {
        public TestCaseProfile()
        {
            CreateMap<GetProblemTestCasesByIdQuery, Testcase>();
            CreateMap<CreateTestcaseCommand, Testcase>();
        }
    }
}
