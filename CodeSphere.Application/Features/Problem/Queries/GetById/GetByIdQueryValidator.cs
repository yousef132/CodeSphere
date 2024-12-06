using CodeSphere.Application.Features.Problem.Queries.GetAll;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
	public class GetByIdQueryValidator : AbstractValidator<GetByIdQuery>
	{
		public GetByIdQueryValidator()
		{

			RuleFor(x => x.ProblemId)
				.NotEmpty()
				.NotNull();

			RuleFor(x => x.UserId)
				.NotEmpty()
				.NotNull();
		}
	}
}
