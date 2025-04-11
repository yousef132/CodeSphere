using FluentValidation;

namespace CodeSphere.Application.Features.Problem.Queries.GetById
{
    public class GetByIdQueryValidator : AbstractValidator<GetProblemByIdQuery>
    {
        public GetByIdQueryValidator()
        {

            RuleFor(x => x.ProblemId)
                .NotEmpty()
                .NotNull();
        }
    }
}
