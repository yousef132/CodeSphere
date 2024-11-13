using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Commands.Register
{


    public sealed record RegisterCommand(
    string Email,
    string Password,
    string UserName) : IRequest<Response>;
}
