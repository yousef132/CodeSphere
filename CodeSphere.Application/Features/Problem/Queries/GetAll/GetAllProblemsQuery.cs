using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllProblemsQuery : IRequest<Response>
    {
        // public string? UserId { get; set; }
        public List<string>? TopicsNames { get; set; }
        public string? ProblemName { get; set; }
        public Difficulty? Difficulty { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
        
        // status : AC , Attempted, Not Attempted
         public string? Status { get; set; }

        // sortBy : Name, Difficulty, acceptanceRate, 
        public string? SortBy { get; set; }

        // order : asc, desc
        public string? Order { get; set; }
        public GetAllProblemsQuery(string? userId,
            List<string>? topicsNames,
            string? problemName,
            Difficulty? difficulty,
            int pageNumber,
            int pageSize,
            string status,
            string sortBy,
            string? order)
        {
            // UserId = userId;
            TopicsNames = topicsNames;
            ProblemName = problemName;
            Difficulty = difficulty;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Status = status;
            SortBy = sortBy;
            Order = order;
        }
    }



}
