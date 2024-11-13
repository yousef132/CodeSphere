using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public sealed record CreateProblemCommand(
    string Name,
    string Description,
    int ContestId,
    string ProblemSetterId,
    string Difficulty,
    decimal RunTimeLimit,
    decimal MemoryLimit) : IRequest<Response>;
}
