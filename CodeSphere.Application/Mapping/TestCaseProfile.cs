using AutoMapper;
using CodeSphere.Application.Features.Problem.Queries.GetProblemTestCasesById;
using CodeSphere.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Mapping
{
    public class TestCaseProfile : Profile
    {
        public TestCaseProfile() =>
            CreateMap<GetProblemTestCasesByIdQuery, Testcase>();
    }
}
