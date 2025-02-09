using CodeSphere.Domain.Abstractions;
using FluentValidation;

namespace CodeSphere.Application.Features.Topic.Commands
{
    public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MustAsync(async (name, cancelation)
                            => !await _unitOfWork
                                        .Repository<Domain.Models.Entities.Topic>()
                                        .AnyAsync(t => t.Name == name))
                                        .WithMessage("the name is already exist");
        }
    }
}
