using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Commands.Register
{
    //public class RegisterCommand
    //{
    //       public string Email { get; set; }
    //       public string Password { get; set; }
    //       public string UserName { get; set; }
    //   }

    public sealed record RegisterCommand(
    string Email,
    string Password,
    string UserName) : IRequest<Response>;
}
