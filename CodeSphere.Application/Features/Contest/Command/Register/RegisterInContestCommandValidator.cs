using FluentValidation;

namespace CodeSphere.Application.Features.Contest.Command.Register
{
    public class RegisterInContestCommandValidator : AbstractValidator<RegisterInContestCommand>
    {
        public RegisterInContestCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
