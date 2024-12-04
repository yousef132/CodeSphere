using CodeSphere.Domain.Models.Entities;
using FluentValidation;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
	public class GetAllProblemsQueryValidator : AbstractValidator<GetAllProblemsQuery>
	{
		public GetAllProblemsQueryValidator()
		{
			RuleFor(x => x.ProblemName).Null().Empty().MinimumLength(5).MaximumLength(30);
			RuleFor(x => x.TopicsIds).Null().Empty();

			RuleFor(x => x.Difficulty)
					   .NotNull()
					   .WithMessage("Difficulty must not be null.") // Ensures it's not null
					   .Must(value => Enum.IsDefined(typeof(Difficulty), value))
					   .WithMessage("Difficulty must be one of the valid values: 0 (Easy), 1 (Medium), or 2 (Hard)."); 
			RuleFor(x => x.UserId).NotEmpty().NotNull();
			
		}
	}



}
