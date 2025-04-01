using CodeSphere.Domain.Premitives;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Application.Features.User.Commnad.EditDetailsQuery
{
    public class EditUserDetailsQuery : IRequest<Response>
    {
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public Gender Gender { get; set; }
    }

    public class EditUserDetailsQueryValidator : AbstractValidator<EditUserDetailsQuery>
    {
        public EditUserDetailsQueryValidator()
        {

        }
    }


}
