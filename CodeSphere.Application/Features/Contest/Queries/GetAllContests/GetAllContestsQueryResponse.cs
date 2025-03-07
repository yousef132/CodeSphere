using CodeSphere.Domain.Premitives;

namespace CodeSphere.Application.Features.Contest.Queries.GetAllContests
{
    public class GetAllContestsQueryResponse
    {
        public int Id { get; set; }
        public ContestStatus ContestStatus { get; set; }

        public string Name { get; set; }

        public bool UserRegistered { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
