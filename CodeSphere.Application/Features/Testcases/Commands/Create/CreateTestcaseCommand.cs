using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.TestCase.Commands.Create
{
    public sealed record CreateTestcaseCommand(
    int ProblemId,
    string Input,
    string expectedOutput) : IRequest<Response>;
}
