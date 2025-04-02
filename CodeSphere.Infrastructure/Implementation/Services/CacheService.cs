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

        public async Task TestCache()
        {
            // Connect to Redis


            string key = "leaderboard";

            // Add players to sorted set (ZADD)
            _Database.SortedSetAdd(key, "Alice", 1500);
            _Database.SortedSetAdd(key, "Bob", 1800);
            _Database.SortedSetAdd(key, "Charlie", 1700);

            // Get sorted elements (ZRANGE with scores)
            Console.WriteLine("Leaderboard:");
            foreach (var entry in _Database.SortedSetRangeByRankWithScores(key, order: StackExchange.Redis.Order.Descending))
            {
                Console.WriteLine($"{entry.Element}: {entry.Score}");
            }

            // Get the rank of a specific player (ZRANK)
            long? rank = _Database.SortedSetRank(key, "Alice", StackExchange.Redis.Order.Descending);
            Console.WriteLine($"Alice's Rank: {rank + 1}");

            // Increment score (ZINCRBY)
            _Database.SortedSetIncrement(key, "Alice", 300);
            Console.WriteLine("Alice's new score: " + _Database.SortedSetScore(key, "Alice"));

            // Get top 2 players (ZRANGE with limit)
            Console.WriteLine("Top 2 Players:");
            var topPlayers = _Database.SortedSetRangeByRankWithScores(key, 0, 1, StackExchange.Redis.Order.Descending);
            foreach (var player in topPlayers)
            {
                Console.WriteLine($"{player.Element}: {player.Score}");
            }
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
            var leaderboard = _Database.SortedSetRangeByRankWithScores(key, 0, -1, StackExchange.Redis.Order.Descending);

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


        public void CacheUserSubmission(SubmissionToCache submission, string userId, int contestId)
        {
            string key = Helper.GenerateUserSubmissionsKey(userId, contestId);

            // Serialize the new submission to a JSON string
            string serializedSubmission = JsonSerializer.Serialize(submission);

            // Append the serialized submission to the list (no need to load the entire list)
            _Database.ListRightPush(key, serializedSubmission);
            SetKeyExpiration(key, 2);
        }



        #region Private
        private void SetKeyExpiration(string key, double time)
             => _Database.KeyExpire(key, TimeSpan.FromHours(time));
        private List<UserProblemSubmissionWithoutUserId> GetUserSubmissionsList(string userId, int contestId)
        {
            var key = Helper.GenerateUserSubmissionsKey(userId, contestId);
            var submissionsStrings = _Database.ListRange(key, 0, -1);

            // o(n) where n is the number of submissions in the list
            var problemSubmissions = new Dictionary<int, ProblemSubmissionsCount>();

            var submissions = submissionsStrings
                .Select(submissionStr => JsonSerializer.Deserialize<SubmissionToCache>(submissionStr))
                .Where(sub => sub != null)
                .ToList();

            foreach (var submission in submissions)
            {
                var count = problemSubmissions.GetValueOrDefault(submission.ProblemId) ?? new ProblemSubmissionsCount();

                if (submission.Result == SubmissionResult.Accepted)
                    count.SuccessCount++;
                else
                    count.FailureCount++;

                problemSubmissions[submission.ProblemId] = count;
            }

            return submissions.Select(sub => new UserProblemSubmissionWithoutUserId
            {
                ProblemId = sub.ProblemId,
                SuccessCount = problemSubmissions[sub.ProblemId].SuccessCount,
                FailureCount = problemSubmissions[sub.ProblemId].FailureCount,
                EarliestSuccessDate = sub.Date,
            }).ToList();
        }

        public bool IsUserSolvedTheProblem(string userId, int contestId, int problemId)
        {
            string key = Helper.GenerateUserSubmissionsKey(userId, contestId);
            var submissionsStrings = _Database.ListRange(key, 0, -1);


            var submissions = submissionsStrings
                .Select(submissionStr => JsonSerializer.Deserialize<SubmissionToCache>(submissionStr))
                .Where(sub => sub != null)
                .ToList();

            return submissions.Any(s => s.ProblemId == problemId && s.Result == SubmissionResult.Accepted);

        }

        #endregion
    }
}