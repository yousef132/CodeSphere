using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public sealed record CreateProblemCommand(
    string Name,
    string Description,
    int ContestId,
    string ProblemSetterId,
    Difficulty Difficulty,
    decimal RunTimeLimit,
    MemoryLimit MemoryLimit,
    List<int> Topics) : IRequest<Response>;
}
