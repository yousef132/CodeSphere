using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllProblemsQuery : IRequest<Response>
    {
        public string? UserId { get; set; }
        public List<int>? TopicsIds { get; set; }
        public string? ProblemName { get; set; }
        public Difficulty? Difficulty { get; set; }

        public GetAllProblemsQuery(string? userId, List<int>? topicsIds, string? problemName, Difficulty? difficulty)
        {
            UserId = userId;
            TopicsIds = topicsIds;
            ProblemName = problemName;
            Difficulty = difficulty;
        }
    }



}
