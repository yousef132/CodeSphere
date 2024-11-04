using AutoMapper;
using CodeSphere.Domain.Abstractions.Repositores;
using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

            var TestCases = _problemRepository.GetTestCasesByProblemId(request.ProblemId);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetProblemTestCasesById
{
    public class GetProblemTestCasesByIdQueryHandler : IRequestHandler<GetProblemTestCasesByIdQuery, Response>
    {
        private readonly IMapper mapper;
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork unitOfWork;

        public GetProblemTestCasesByIdQueryHandler(IMapper mapper,
                                                   IProblemRepository problemRepository,
                                                   IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            _problemRepository = problemRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(GetProblemTestCasesByIdQuery request, CancellationToken cancellationToken)
        {
            var problem = await unitOfWork.Repository<Domain.Models.Entities.Problem>().GetByIdAsync(request.ProblemId);
            if (problem == null)
                return await Response.FailureAsync("Problem not found");

            var TestCases = _problemRepository.GetTestCasesByProblemId(request.ProblemId);

            return await Response.SuccessAsync(TestCases, "TestCases fetched successfully");
        }
    }
}
