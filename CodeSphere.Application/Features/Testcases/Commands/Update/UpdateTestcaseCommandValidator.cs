using FluentValidation;

namespace CodeSphere.Application.Features.Testcases.Commands.Update
{
    public class UpdateTestcaseCommandValidator : AbstractValidator<UpdateTestcaseCommand>
    {
        public UpdateTestcaseCommandValidator()
        {
            RuleFor(x => x.TestcaseId)
                .GreaterThan(0).WithMessage("TestcaseId is required.");

            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("ProblemId is required.");

            RuleFor(x => x.Input)
                .NotEmpty().WithMessage("Input is required.")
                .MinimumLength(1).WithMessage("Input must be at least 1 character long.");

            RuleFor(x => x.Output)
                .NotEmpty().WithMessage("Expected output is required.")
                .MinimumLength(1).WithMessage("Output must be at least 1 character long.");
        }
    }
}
