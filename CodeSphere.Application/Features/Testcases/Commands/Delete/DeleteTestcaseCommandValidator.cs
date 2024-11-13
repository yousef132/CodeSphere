using FluentValidation;

namespace CodeSphere.Application.Features.Testcases.Commands.Delete
{
    public class DeleteTestcaseCommandValidator : AbstractValidator<DeleteTestcaseCommand>
    {
        public DeleteTestcaseCommandValidator()
        {
            RuleFor(x => x.TestcaseId)
                .NotEmpty().WithMessage("TestcaseId is required.");
        }
    }
}
