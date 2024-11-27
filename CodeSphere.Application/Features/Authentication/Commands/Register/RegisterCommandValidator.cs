using FluentValidation;

namespace CodeSphere.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must be Not Empty")
                .NotNull().WithMessage("Email must Not be Null")
                .EmailAddress().WithMessage("EmailAddress must Not be Empty")
                .Length(5, 50).WithMessage("Email must not be less than 5 and not more than 50.");



            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName must Not be Empty")
                .NotNull().WithMessage("UserName must Not be Null")
                .Length(5, 50).WithMessage("UserName must not be less than 5 and not more than 50.");




            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must Not be Empty")
                .NotNull().WithMessage("Password must Not be Null")
                .Length(5, 50).WithMessage("Password must not be less than 5 and not more than 50.");



        }
    }
}
