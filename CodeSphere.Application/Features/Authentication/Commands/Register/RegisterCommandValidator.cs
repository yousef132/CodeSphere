using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email must be Not Empty")
				.NotNull().WithMessage("Email must Not be Null")
				.EmailAddress().WithMessage("EmailAddress must Not be Empty")
				.MinimumLength(5).WithMessage("MinimumLength must be 5")
				.MaximumLength(50).WithMessage("MaximumLength must be 50");
          
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName must Not be Empty")
				.NotNull().WithMessage("UserName must Not be Null")
				.MinimumLength(5).WithMessage("MinimumLength must be 5")
				.MaximumLength(50).WithMessage("MaximumLength must be 50");
           
            
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password must Not be Empty")
                .NotNull().WithMessage("Password must Not be Null")
				.MinimumLength(5).WithMessage("Password must be Not 5")
				.MaximumLength(50).WithMessage("MaximumLength must 50")
				.WithMessage("Password must be between 5 and 50 characters long.");


        }
    }
}
