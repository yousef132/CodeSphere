using CodeSphere.Domain.Abstractions.Services;
using FluentValidation;

namespace CodeSphere.Application.Features.Problem.Commands.SolveProblem
{
    public class SubmitSolutionCommandValidator : AbstractValidator<SubmitSolutionCommand>
    {
        private readonly IFileService fileService;

        public SubmitSolutionCommandValidator(IFileService fileService)
        {

            this.fileService = fileService;
            CustomValidator();
        }


        public void CustomValidator()
        {

            RuleFor(x => x)
           .NotEmpty().WithMessage("Input cannot be empty.")
           .NotNull().WithMessage("Input cannot be null.")
           .Must((model) => fileService.CheckFileExtension(model.Code, model.Language))
           .WithMessage("File extension doesn't match the requested language!");

            RuleFor(x => x)
                .NotEmpty().WithMessage("Input cannot be empty.")
                .NotNull().WithMessage("Input cannot be null.")
                .Must((model) => fileService.CheckFileExtension(model.Code, model.Language))
                .WithMessage("Unsupported language!");

            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.ProblemId).NotEmpty().NotNull();
            RuleFor(x => x.ContestId).NotEmpty().NotNull();



        }


        //public IFormFile Code { get; set; }
        //public int? ContestId { get; set; }
        //public Language Language { get; set; }
    }
}
