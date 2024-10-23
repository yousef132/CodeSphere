using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Commands.Create
{
    public sealed record CreateProblemCommand(
    string Name,
    string Description,
    float Rate) : IRequest<Response>;
}
