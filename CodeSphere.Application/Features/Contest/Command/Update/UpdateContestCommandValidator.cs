using FluentValidation;


namespace CodeSphere.Application.Features.Contest.Command.Update
{
    public class UpdateContestCommandValidator : AbstractValidator<UpdateContestCommand>
    {
        public UpdateContestCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.StartTime).NotEmpty().NotNull();
            RuleFor(x => x.EndTime).NotEmpty().NotNull();
        }
    }
}
