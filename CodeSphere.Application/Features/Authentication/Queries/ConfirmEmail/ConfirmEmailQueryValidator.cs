using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Queries.ConfirmEmail
{
	public class ConfirmEmailQueryValidator :AbstractValidator<ConfirmEmailQuery>
	{
		public ConfirmEmailQueryValidator()
		{
			RuleFor(x => x.UserId)
				.NotEmpty().WithMessage("UserId must be Not Empty")
				.NotNull().WithMessage("UserId must Not be Null");


			RuleFor(x => x.Code)
				.NotEmpty().WithMessage("Code must Not be Empty")
				.NotNull().WithMessage("Code must Not be Null");

		}
	}
}
