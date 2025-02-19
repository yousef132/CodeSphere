using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Queries
{
    public class GetContestProblemsQuery : IRequest<Response>
    {
        public int Id { get; set; }

        public GetContestProblemsQuery(int id)
        {
            Id = id;
        }
    }


}
