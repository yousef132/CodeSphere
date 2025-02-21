using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Application.Features.Problem.Commands.SolveProblem
{
    public class SubmitSolutionCommand : IRequest<Response>
    {
        public int ProblemId { get; set; }
        public IFormFile Code { get; set; }
        public int ContestId { get; set; }
        public Language Language { get; set; }
    }
}
