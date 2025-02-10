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
                .Must(status => !status.HasValue || Enum.IsDefined(typeof(Status), status.Value))
                .WithMessage("Status must be either 0 (AC), 1 (Attempted), 2 (Not Attempted), or it can be null.");

            RuleFor(x => x.SortBy)
                .Must(sortBy => !sortBy.HasValue || Enum.IsDefined(typeof(SortBy), sortBy.Value))
                .WithMessage("SortBy must be either 0 (Name), 1 (Difficulty), 2 (AcceptanceRate), or it can be null.");

            RuleFor(x => x.Order)
                .Must(order => !order.HasValue || Enum.IsDefined(typeof(Order), order.Value))
                .WithMessage("Order must be either 0 (Ascending) or 1 (Descending), or it can be null.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page size must be greater than or equal to 1.");

            RuleFor(x => x.TopicsNames)
                .Must(topicsNames => topicsNames == null || topicsNames.Count > 0)
                .WithMessage("Topics names must be null or have at least one element.");

            RuleFor(x => x.ProblemName)
                .Must(problemName => problemName == null || problemName.Length > 0)
                .WithMessage("Problem name must be null or have at least one character.");


        }
    }
}
