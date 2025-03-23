using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Responses.Contest;
using CodeSphere.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class ContestRepository : IContestRepository
    {
        private readonly ApplicationDbContext context;

        public ContestRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Task<IReadOnlyList<(Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        //public async Task<IReadOnlyList<(Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId)
        //    => await context.Contests.Include(c => c.Registrations.Where(r => r.UserId == userId)).Select(c=>
        //    {
        //        var isRegistered = c.Registrations.Any();
        //        return (c, isRegistered);
        //    }).ToListAsync();

        public async Task<IReadOnlyList<Problem>> GetContestProblemsByIdAsync(int contestId)

             => await context.Problems.Where(x => x.ContestId == contestId).ToListAsync();

        public async Task<IReadOnlyList<StandingDto>> GetContestStanding(int contestId)
        {
            #region MyRegion
            //join contest with submits and group by user id
            //var standing = from c in context.Contests
            //               join s in context.Submits on c.Id equals s.ContestId
            //               join u in context.Users on s.UserId equals u.Id
            //               where c.Id == contestId
            //               select new StandingDto
            //               {
            //                   UserId = s.UserId,
            //                   UserImage = u.ImagePath,
            //                   UserName = u.UserName,
            //                   UserProblemSubmissions = new List<Dictionary<int, UserProblemSubmission>>()
            //                  {
            //                          new Dictionary<int, UserProblemSubmission>()
            //                          {
            //                              {s.ProblemId, new UserProblemSubmission()
            //                              {
            //                                  SubmissionId = s.Id,
            //                                  SubmissionDate = s.SubmissionDate,
            //                                  Language = s.Language,
            //                                  FailCount = s.Result != SubmissionResult.Accepted ? 1 : 0
            //                              }
            //                              // key with the problemId
            //                              }
            //                          }
            //                  }
            //               };


            #endregion

            return standing.ToList();
            #region with groupby
            //var standing = from c in context.Contests
            //               join s in context.Submits on c.Id equals s.ContestId
            //               join u in context.Users on s.UserId equals u.Id
            //               where c.Id == contestId
            //               group s by new { s.UserId, u.ImagePath, u.UserName } into userGroup
            //               select new StandingDto
            //               {
            //                   UserId = userGroup.Key.UserId,
            //                   UserImage = userGroup.Key.ImagePath,
            //                   UserName = userGroup.Key.UserName,
            //                   UserProblemSubmissions = new List<Dictionary<int, UserProblemSubmission>>
            //                   {
            //                        userGroup.GroupBy(s => s.ProblemId).ToDictionary(
            //                            x => x.Key, x => new UserProblemSubmission
            //                        {
            //                            SubmissionId = x.FirstOrDefault().Id,
            //                            SubmissionDate = x.FirstOrDefault().SubmissionDate,
            //                            Language = x.FirstOrDefault().Language,
            //                            FailCount = x.Count(s => s.Result != SubmissionResult.Accepted)
            //                        })
            //                   }


            //               };

            #endregion

        }
    }


}