﻿using FluentValidation;

namespace CodeSphere.Application.Features.Contest.Queries.GetContestProblems
{
    public class GetContestProblemsQueryValidator : AbstractValidator<GetContestProblemsQuery>
    {
        public GetContestProblemsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

}
