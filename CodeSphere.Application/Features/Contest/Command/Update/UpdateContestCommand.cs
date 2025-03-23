using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Command.Update
{
    public class UpdateContestCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
