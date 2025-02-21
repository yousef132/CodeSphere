using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Contest.Command.Register
{
    public class RegisterInContestCommand : IRequest<Response>
    {
        public int Id { get; set; }

        public RegisterInContestCommand(int id)
        {
            this.Id = id;
        }
    }
}
