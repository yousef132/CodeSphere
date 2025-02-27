using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Plagiarism.Queries.GetByContestIdQuery
{
    public class GetByContestIdQuery : IRequest<Response>
    {
        public int ContestId { get; set; }
        public int threshold { get; set; }
        public List<int> ProblemIds { get; set; }
        public GetByContestIdQuery(int contestId, int threshold, List<int> problemIds)
        {
            ContestId = contestId;
            this.threshold = threshold;
            ProblemIds = problemIds;
        }
    }
}
