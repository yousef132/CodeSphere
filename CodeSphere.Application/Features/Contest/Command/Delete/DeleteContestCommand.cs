using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Command.Delete
{
    public class DeleteContestCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
