using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using FluentValidation;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public class CreateProblemCommandValidator : AbstractValidator<CreateProblemCommand>
    {
        public CreateProblemCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(5).MaximumLength(30);
            RuleFor(x => x.Description).NotEmpty().NotNull().MinimumLength(10).MaximumLength(4000);
			
            RuleFor(x => x.Difficulty)
					   .NotNull()
					   .WithMessage("Difficulty must not be null.") // Ensures it's not null
					   .Must(value => Enum.IsDefined(typeof(Difficulty), value))
					   .WithMessage("Difficulty must be one of the valid values: 0 (Easy), 1 (Medium), or 2 (Hard)."); RuleFor(x => x.ContestId).NotEmpty().NotNull();
            RuleFor(x => x.ProblemSetterId).NotEmpty().NotNull();
            RuleFor(x => x.RunTimeLimit).NotNull();
			
			RuleFor(x => x.MemoryLimit)
					   .NotNull()
					   .WithMessage("MemoryLimit must not be null.") // Ensures it's not null
					   .Must(value => Enum.IsDefined(typeof(MemoryLimit), value))
					   .WithMessage("MemoryLimit must be one of the valid values: 16 (Lowest), 32 (Low), 64 (Medium), 128 (High), or 256 (Highest).");
		}
    }
}
