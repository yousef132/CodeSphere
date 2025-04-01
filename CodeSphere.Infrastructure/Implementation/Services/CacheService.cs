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

        public void UpdateStanding(ContestPoints points, UserToCache user, int ContestId)
        {
            string key = Helper.GenerateContestKey(ContestId);
            string member = Helper.ConvertUserToRedisMemeber(user);

            // Check if the user exists in the leaderboard
            long? rank = _Database.SortedSetRank(key, member, StackExchange.Redis.Order.Descending);

            // Add user first time
            if (rank is null)
                _Database.SortedSetAdd(key, member, (int)points);

            // Increase score
            _Database.SortedSetIncrement(key, member, (int)points);

            //  expiration for 2 hours 
            _Database.KeyExpire(key, TimeSpan.FromHours(2));
        }
        public void CacheUserSubmission(SubmissionToCache submission, int contestId)
        {
            string key = Helper.GenerateUserSubmissionKey(submission.UserId, contestId);

            // Serialize the new submission to a JSON string
            string serializedSubmission = JsonSerializer.Serialize(submission);

            // Append the serialized submission to the list (no need to load the entire list)
            _Database.ListRightPush(key, serializedSubmission);
        }


        public async Task<IReadOnlyList<ContestStandingResposne>> GetContestStanding(int contestId, int start, int stop)
        {
            string key = Helper.GenerateContestKey(contestId);
            // o(log n + m) where n is the number of elements in the sorted set and m the number of elements returned.
            var leaderboard = _Database.SortedSetRangeByRankWithScores(key, start, stop, StackExchange.Redis.Order.Descending);


            // foreach returned element get the submsission list 
            return leaderboard.Select(entry =>
            {
                var parts = entry.Element.ToString().Split('|');
                return new StandingDto
                {
                    UserId = parts[0],
                    UserName = parts[1],
                    ImagePath = parts[2],
                    TotalPoints = (int)entry.Score
                };
            }).ToList();
        }
    }
}
