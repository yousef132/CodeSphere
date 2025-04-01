using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using FluentValidation;
using System.Text.Json;

namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommandValidator : AbstractValidator<RunCodeCommand>
    {
        public RunCodeCommandValidator()
        {
            RuleFor(x => x.Language)
                .IsInEnum()
                .WithMessage("Invalid language value. Must be a defined enum.");

            // Validate Code
            RuleFor(x => x)
                .Must(x => Helper.ValidateFile(x.Language, 5, 0, x.Code))
                .WithMessage("Code file cannot be empty.");

            // Validate ProblemId
            RuleFor(x => x.ProblemId)
                .NotEmpty().NotNull()
                .GreaterThan(0)
                .WithMessage("ProblemId must be greater than 0.");

            // Validate CustomTestcasesJson
            RuleFor(x => x.CustomTestcasesJson)
                .NotEmpty()
                .WithMessage("CustomTestcasesJson is required.")
                .Must(BeValidJsonArray)
                .WithMessage("CustomTestcasesJson must be a valid JSON array.");
        }
        private bool BeValidJsonArray(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return false;

            try
            {
                var testcases = JsonSerializer.Deserialize<List<CustomTestcaseDto>>(json);
                return testcases != null;
            }
            catch
            {
                return false;
            }
        }
    }


}
