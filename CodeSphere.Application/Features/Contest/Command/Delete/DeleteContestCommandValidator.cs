using FluentValidation;

namespace CodeSphere.Application.Features.Contest.Command.Delete
{
    public class DeleteContestCommandValidator : AbstractValidator<DeleteContestCommand>
    {
        public DeleteContestCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
