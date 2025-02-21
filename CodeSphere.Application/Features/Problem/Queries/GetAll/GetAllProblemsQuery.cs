using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using MediatR;

namespace CodeSphere.Application.Features.Problem.Queries.GetAll
{
    public class GetAllProblemsQuery : IRequest<Response>
    {
        // public string? UserId { get; set; }
        public List<int>? Topics { get; set; }
        public string? ProblemName { get; set; }
        public Difficulty? Difficulty { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;


        // status : AC , Attempted, Not Attempted
        public ProblemStatus? Status { get; set; }

        // sortBy : Name, Difficulty, acceptanceRate, 
        public SortBy SortBy { get; set; }

        // order : asc, desc
        public Order Order { get; set; }
        public GetAllProblemsQuery(string? userId,
            List<int>? topicsNames,
            string? problemName,
            Difficulty? difficulty,
            int pageNumber,
            int pageSize,
            ProblemStatus? status,
            SortBy sortBy,
            Order order)
        {
            // UserId = userId;
            Topics = topicsNames;
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
