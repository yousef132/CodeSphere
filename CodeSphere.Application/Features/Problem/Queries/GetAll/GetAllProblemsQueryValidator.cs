using CodeSphere.Domain.Models.Entities;
using FluentValidation;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllProblemsQueryValidator : AbstractValidator<GetAllProblemsQuery>
    {
        public GetAllProblemsQueryValidator()
        {

            RuleFor(x => x.Difficulty)
                .Must(status => !status.HasValue || Enum.IsDefined(typeof(Difficulty), status.Value))
                .WithMessage("Difficulty must be either 0 (Easy), 1 (Medium), or 2 (Hard), or it can be null.");

            RuleFor(x => x.Status)
                .Must(status => !status.HasValue || Enum.IsDefined(typeof(ProblemStatus), status.Value))
                .WithMessage("Status must be either 0 (AC), 1 (Attempted), 2 (Not Attempted), or it can be null.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size must be greater than or equal to 1.");

            RuleFor(x => x.ProblemName)
                .Must(problemName => problemName == null || problemName.Length > 0)
                .WithMessage("Problem name must be null or have at least one character.");


        }
    }
}
