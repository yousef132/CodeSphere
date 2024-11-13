using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Testcases.Commands.Delete
{
    public sealed record DeleteTestcaseCommand(int TestcaseId) : IRequest<Response>;
}
