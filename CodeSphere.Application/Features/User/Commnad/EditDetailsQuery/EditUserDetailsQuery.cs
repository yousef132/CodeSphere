using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Application.Features.User.Commnad.EditDetailsQuery
{
    public class EditUserDetailsQuery : IRequest<Response>
    {
        public string UserId { get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public Gender Gender { get; set; }
    }

}
