using CodeSphere.Domain.Abstractions.Repositories;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Responses.Contest;
using CodeSphere.Infrastructure.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CodeSphere.Infrastructure.Implementation.Repositories
{
    public class ContestRepository : IContestRepository
    {
        private readonly ApplicationDbContext context;

        public ContestRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<IReadOnlyList<Tuple<Contest, bool>>> GetAllContestWithRegisteredUserAsync(string? userId)
        {
            return await context.Contests
                .Include(c => c.Registrations.Where(r => r.UserId == userId))
                .Select(c => new Tuple<Contest, bool>(c, c.Registrations.Any())).ToListAsync();
        }

        //public async Task<IReadOnlyList<(Contest, bool)>> GetAllContestWithRegisteredUserAsync(string userId)
        //    => await context.Contests.Include(c => c.Registrations.Where(r => r.UserId == userId)).Select(c=>
        //    {
        //        var isRegistered = c.Registrations.Any();
        //        return (c, isRegistered);
        //    }).ToListAsync();

        public async Task<IReadOnlyList<Problem>> GetContestProblemsByIdAsync(int contestId)

             => await context.Problems.Where(x => x.ContestId == contestId).ToListAsync();

        public async Task<IReadOnlyList<StandingDto>> GetContestStanding(int contestId, int index, int pageSize)
        {
            var result1 = await context.Database.SqlQuery<ContestStandingResposne>($@"
                                          SELECT 
                                              UniqueSubmissions.UserId, 
                                              u.UserName,
                                              u.ImagePath,
                                              SUM(p.ContestPoints) AS TotalPoints
                                          FROM (
                                        		-- get first accepted submission for each problem and user
                                              SELECT MIN(s.SubmissionDate) AS FirstSubmission, s.UserId, s.ProblemId
                                              FROM  Submits s 
                                              JOIN Contests c ON s.ContestId = c.Id
                                              WHERE s.ContestId = {contestId} 
                                                AND s.Result = 0  
                                                AND s.SubmissionDate BETWEEN c.StartDate AND c.EndDate
                                              GROUP BY s.UserId, s.ProblemId
                                          ) AS UniqueSubmissions
                                          JOIN Problems p ON UniqueSubmissions.ProblemId = p.Id join AspNetUsers u  on UniqueSubmissions.userid = u.id
                                          GROUP BY UniqueSubmissions.UserId,u.UserName,u.ImagePath
                                          ORDER BY TotalPoints DESC, MIN(UniqueSubmissions.FirstSubmission) ASC
                                          ").ToListAsync();//OFFSET 1 ROWS FETCH NEXT 11 ROWS ONLY;

            var usersRanking = result1.Select(x => new StandingDto
            {
                UserId = x.UserId,
                TotalPoints = x.TotalPoints,
                ImagePath = x.ImagePath,
                UserName = x.UserName
            }).ToList();



            if (!usersRanking.Any())
                return usersRanking;

            string usersIdString = string.Join("|", usersRanking.Select(s => s.UserId));

            // get all the submissions for the users
            var result = await context.Database
                       .SqlQueryRaw<UserProblemSubmission>(
                           "EXEC GetProblemSubmissionsCountByProblemAndUser @ContestId, @UsersId",
                           new SqlParameter("@ContestId", contestId),
                           new SqlParameter("@UsersId", usersIdString))
                       .ToListAsync();


            foreach (var user in usersRanking)
                user.UserProblemSubmissions = result.Where(r => r.UserId == user.UserId).Select(r => new UserProblemSubmissionWithoutUserId
                {
                    FailureCount = r.FailureCount,
                    ProblemId = r.ProblemId,
                    EarliestSuccessDate = r.EarliestSuccessDate,
                    SuccessCount = r.SuccessCount

                }).ToList();


            return usersRanking;
        }

        public async Task<bool> IsRegistered(string userId, int contestId)
         => await context.Registers.AnyAsync(x => x.UserId == userId && x.ContestId == contestId);
    }


}