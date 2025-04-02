using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using CodeSphere.Domain.Requests;
using CodeSphere.Domain.Responses.Contest;
using StackExchange.Redis;
using System.Text.Json;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _Database;

        public CacheService(IConnectionMultiplexer multiplexer)
        {
            _Database = multiplexer.GetDatabase();
        }



        public async Task CacheResponseAsync(string key, object Response, TimeSpan timeToiLive)
        {
            if (Response is null)
                return;

            var serializeOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(Response, serializeOptions);
            await _Database.StringSetAsync(key, serializedResponse, timeToiLive);
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var value = await _Database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return null;

            return (value);
        }

        public async Task UpdateContestCache(Submit submission)
        {
            string userKey = $"leaderboard:user:{submission.UserId}";
            string globalKey = "leaderboard:global";

            string problemField = $"problem:{submission.ProblemId}";
            string submissionData = $"{submission.Id},{submission.SubmissionDate:O},{(int)submission.Result}";

            var db = _Database;

            // Start Redis transaction
            var tran = db.CreateTransaction();

            // Check if the problem was already solved
            bool alreadyAccepted = false;
            var existingSubmission = await db.HashGetAsync(userKey, problemField);
            if (existingSubmission.HasValue)
            {
                var submissionParts = existingSubmission.ToString().Split(',');
                if (submissionParts.Length > 1 && Enum.TryParse(submissionParts[1], out SubmissionResult result))
                {
                    alreadyAccepted = (result == SubmissionResult.Accepted);
                }
            }

            // Update User-Specific Hash (always update)
            tran.HashSetAsync(userKey, problemField, submissionData);

            // Update Global Score only if it's the first accepted submission for this problem
            if (submission.Result == SubmissionResult.Accepted && !alreadyAccepted)
            {
                tran.SortedSetIncrementAsync(globalKey, submission.UserId, 1);
            }

            // Execute transaction
            await tran.ExecuteAsync();
        }

        public void CacheContestStanding(ContestPoints points, UserToCache user, int ContestId)
        {
            string key = Helper.GenerateContestKey(ContestId);
            //string member = Helper.ConvertUserToRedisMemeber(user);
            var serializedUser = JsonSerializer.Serialize(user);

            // Increase or add the user with their points
            _Database.SortedSetIncrement(key, serializedUser, (int)points);

            SetKeyExpiration(key, 2);
        }

        public IReadOnlyList<StandingDto> GetContestStanding(int contestId, int start, int stop)
        {
            // total complexity is o(log n + m) * o(s) where is the NO submission in the list for each user in the leadboard

            string key = Helper.GenerateContestKey(contestId);
            // o(log n + m) where n is the number of elements in the sorted set and m the number of elements returned.
            var leaderboard = _Database.SortedSetRangeByRankWithScores(key, start, stop, StackExchange.Redis.Order.Descending);

            // foreach returned user get the submsissions list 
            return leaderboard.Select(entry =>
            {

                var user = JsonSerializer.Deserialize<UserToCache>(entry.Element);
                return new StandingDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    ImagePath = user.ImagePath,
                    TotalPoints = (int)entry.Score,
                    UserProblemSubmissions = GetUserSubmissionsList(user.UserId, contestId)
                };
            }).ToList();
        }

        public void CacheUserSubmission(SubmissionToCache submissionToCache, string userId, int contestId)
        {
            // how data looks like in redis : 
            //Key: user:123:contest:5:submissions
            //------------------------------------
            //Field: "102" (ProblemId)  →  Value: { "SuccessCount":2, "FailureCount":3}
            // Field: "205" (ProblemId) →   Value: { "SuccessCount":1, "FailureCount":5}
            string key = Helper.GenerateUserSubmissionsKey(userId, contestId);
            string field = submissionToCache.ProblemId.ToString();

            var existingSubmissionJson = _Database.HashGet(key, field);


            if (!existingSubmissionJson.IsNullOrEmpty)
            {
                // update the existing submission

                var existingSubmission = JsonSerializer.Deserialize<ProblemSubmissionsCount>(existingSubmissionJson);
                if (submissionToCache.Result == SubmissionResult.Accepted)
                {
                    existingSubmission.SuccessCount++;
                    existingSubmission.EarliestSuccessDate = existingSubmission.EarliestSuccessDate == null || existingSubmission.EarliestSuccessDate > submissionToCache.Date
                        ? submissionToCache.Date
                        : existingSubmission.EarliestSuccessDate;
                }
                else
                    existingSubmission.FailureCount++;

                //    Key=> user:123:contest:5:submissions  field=> PID , value=> { "SuccessCount":2, "FailureCount":3,"EarliestSuccessDate":""}
                _Database.HashSet(key, field, JsonSerializer.Serialize(existingSubmission));

            }
            else
            {
                // add first submission for this problem
                var newSubmission = new ProblemSubmissionsCount
                {
                    EarliestSuccessDate = submissionToCache.Result == SubmissionResult.Accepted ? submissionToCache.Date : null,
                    FailureCount = submissionToCache.Result == SubmissionResult.Accepted ? 0 : 1,
                    SuccessCount = submissionToCache.Result == SubmissionResult.Accepted ? 1 : 0
                };
                _Database.HashSet(key, field, JsonSerializer.Serialize(newSubmission));



                // TODO : update the sorted set if this approach is not bad because of signle responsipily

                //CacheContestStanding(, new Domain.Requests.UserToCache
                //{
                //    UserId = UserId,
                //    ImagePath = user.FindFirst("ImagePath")?.Value,
                //    UserName = user.FindFirstValue(ClaimTypes.Name),
                //}, submissionToCache.ProblemId);


            }

            SetKeyExpiration(key, 2);

        }

        #region Private
        private void SetKeyExpiration(string key, double time)
             => _Database.KeyExpire(key, TimeSpan.FromHours(time));
        private List<UserProblemSubmissionWithoutUserId> GetUserSubmissionsList(string userId, int contestId)
        {
            var key = Helper.GenerateUserSubmissionsKey(userId, contestId);

            var submissionsStrings = _Database.HashGetAll(key);

            return submissionsStrings.Select((s) =>
            {
                var problemId = int.Parse(s.Name);
                var submission = JsonSerializer.Deserialize<ProblemSubmissionsCount>(s.Value);
                return new UserProblemSubmissionWithoutUserId
                {
                    ProblemId = problemId,
                    SuccessCount = submission.SuccessCount,
                    FailureCount = submission.FailureCount,
                    EarliestSuccessDate = submission.EarliestSuccessDate
                };
            }).ToList();

            #region Using List
            ////var submissionsStrings = _Database.ListRange(key, 0, -1);

            ////var problemSubmissions = new Dictionary<int, ProblemSubmissionsCount>();
            ////SubmissionToCache submission = null;
            ////foreach (var submissionStr in submissionsStrings)
            ////{
            ////    submission = JsonSerializer.Deserialize<SubmissionToCache>(submissionStr);
            ////    if (submission == null) continue;

            ////    if (!problemSubmissions.TryGetValue(submission.ProblemId, out var count))
            ////    {
            ////        count = new ProblemSubmissionsCount();
            ////        problemSubmissions[submission.ProblemId] = count;
            ////    }

            ////    if (submission.Result == SubmissionResult.Accepted)
            ////    {
            ////        count.SuccessCount++;
            ////        count.EarliestSuccessDate = count.EarliestSuccessDate == null || count.EarliestSuccessDate > submission.Date
            ////            ? submission.Date
            ////            : count.EarliestSuccessDate;
            ////    }
            ////    else
            ////    {
            ////        count.FailureCount++;
            ////    }
            ////}

            ////return problemSubmissions.Select(kvp => new UserProblemSubmissionWithoutUserId
            ////{
            ////    ProblemId = kvp.Key,
            ////    SuccessCount = kvp.Value.SuccessCount,
            ////    FailureCount = kvp.Value.FailureCount,
            ////    EarliestSuccessDate = kvp.Value.EarliestSuccessDate
            ////}).ToList(); 
            #endregion
        }


        public bool IsUserSolvedTheProblem(string userId, int contestId, int problemId)
        {
            string key = Helper.GenerateUserSubmissionsKey(userId, contestId);
            var submissionJson = _Database.HashGet(key, problemId.ToString());

            if (submissionJson.IsNullOrEmpty)
                return false;

            // Deserialize the submission
            var submission = JsonSerializer.Deserialize<ProblemSubmissionsCount>(submissionJson!);

            // Check if the problem was solved
            return submission.SuccessCount > 0;

        }

        #endregion
    }
}