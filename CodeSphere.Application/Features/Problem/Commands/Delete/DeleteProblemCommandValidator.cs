using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Problem.Commands.Delete
{
    public class DeleteProblemCommandValidator : AbstractValidator<DeleteProblemCommand>
    {
        public DeleteProblemCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id shouldn't be empty").
                NotNull().WithMessage("Id shouldn't be null").
                GreaterThan(0).WithMessage("Id should be greater than 0");

        }
    }
}
