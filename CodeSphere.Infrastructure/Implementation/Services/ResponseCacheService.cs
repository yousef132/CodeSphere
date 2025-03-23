using CodeSphere.Domain.Abstractions.Services;
using CodeSphere.Domain.Models.Entities;
using CodeSphere.Domain.Premitives;
using StackExchange.Redis;
using System.Text.Json;

namespace CodeSphere.Infrastructure.Implementation.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _Database;

        public ResponseCacheService(IConnectionMultiplexer multiplexer)
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

    }
}
