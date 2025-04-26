using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Command.Create
{
    public class CreateContestCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
