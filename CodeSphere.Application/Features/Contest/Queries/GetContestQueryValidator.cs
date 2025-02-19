using FluentValidation;

namespace CodeSphere.Application.Features.Contest.Queries
{
    public class GetContestQueryValidator : AbstractValidator<GetContestProblemsQuery>
    {
        public GetContestQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

}
