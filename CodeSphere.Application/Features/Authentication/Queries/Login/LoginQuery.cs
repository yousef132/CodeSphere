using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Queries.Login
{

public sealed record LoginQuery(
	string Email,
	string Password) : IRequest<Response>;
}
