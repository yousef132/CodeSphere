using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.TestCase.Commands.Create
{
    public sealed record CreateTestcaseCommand(
    Guid ProblemId,
    string Input,
    string Output) : IRequest<Response>;
}
