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
        }
    }
}
