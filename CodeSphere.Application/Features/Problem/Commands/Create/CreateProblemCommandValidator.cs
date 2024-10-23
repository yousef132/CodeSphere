using FluentValidation;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public class CreateProblemCommandValidator : AbstractValidator<CreateProblemCommand>
    {
        public CreateProblemCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(5).MaximumLength(30);
            RuleFor(x => x.Description).NotEmpty().NotNull().MinimumLength(10).MaximumLength(300);
            RuleFor(x => x.Rate).NotEmpty().NotNull();

        }



    }
}
