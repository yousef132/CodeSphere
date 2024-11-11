using CodeSphere.Domain.Premitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Queries.ConfirmEmail
{
	public record ConfirmEmailQuery (
		string UserId,
		string Code
		) : IRequest<Response>;

}
