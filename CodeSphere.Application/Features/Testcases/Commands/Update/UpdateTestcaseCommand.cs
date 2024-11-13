using CodeSphere.Domain.Premitives;
using MediatR;


namespace CodeSphere.Application.Features.Testcases.Commands.Update
{
    public sealed record UpdateTestcaseCommand(
        int TestcaseId,
        int ProblemId,
        string Input,
        string Output) : IRequest<Response>;
}
