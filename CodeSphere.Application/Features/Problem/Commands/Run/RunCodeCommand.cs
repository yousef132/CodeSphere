using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CodeSphere.Application.Features.Problem.Commands.Run
{
    public class RunCodeCommand : IRequest<Response>
    {
        public Language Language { get; set; }
        public IFormFile Code { get; set; }
        public int ProblemId { get; set; }
        public string CustomTestcasesJson { get; set; }
    }
}
