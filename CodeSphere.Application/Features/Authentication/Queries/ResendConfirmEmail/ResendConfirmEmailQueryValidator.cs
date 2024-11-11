using FluentValidation;

namespace CodeSphere.Application.Features.Authentication.Queries.ResendConfirmEmail
{
	public class ResendConfirmEmailQueryValidator : AbstractValidator<ResendConfirmEmailQuery>
	{
        public ResendConfirmEmailQueryValidator()
        {
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email must be Not Empty")
				.NotNull().WithMessage("Email must Not be Null")
				.EmailAddress().WithMessage("Must Be Email");
		}
    }
}
