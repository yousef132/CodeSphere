using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Email.Command.Send
{
    public class SendEmailCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
