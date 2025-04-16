using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Testcases.Commands.Update
{
    public sealed record UpdateTestcaseCommand(
        int TestcaseId,
        string Input,
        string ExpectedOutput) : IRequest<Response>;
}
