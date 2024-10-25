using CodeSphere.Application.Features.Authentication.Commands.Register;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Queries.Login
{
	public class LoginQueryValidator : AbstractValidator<LoginQuery>
	{
        public LoginQueryValidator()
        {
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email must be Not Empty")
				.NotNull().WithMessage("Email must Not be Null")
				.EmailAddress().WithMessage("EmailAddress must Not be Empty")
				.MinimumLength(5).WithMessage("MinimumLength must be 5")
				.MaximumLength(50).WithMessage("MaximumLength must be 50");

			RuleFor(x=>x.Password).NotEmpty().WithMessage("Password must Not be Empty")
				.NotNull().WithMessage("Password must Not be Null")
				.MinimumLength(5).WithMessage("MinimumLength must be 5 ")
				.MaximumLength(50).WithMessage("MaximumLength must be 50 ");
		}
    }
}
