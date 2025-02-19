using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;

namespace CodeSphere.WebApi.Filters
{
    public class RateLimitingFilter : Attribute, IAsyncActionFilter
    {
        private static readonly ConcurrentDictionary<string, RateLimitInfo> RateLimitStore = new();
        private int _maxRequestsPerMinute;
        private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1);

        public RateLimitingFilter(int MaxRequestsPerMinute)
        {
            this._maxRequestsPerMinute = MaxRequestsPerMinute;
        }

        private bool IsRateLimited(string ipAddress)
        {
            var currentTime = DateTime.UtcNow;

            var rateLimitInfo = RateLimitStore.GetOrAdd(ipAddress, new RateLimitInfo());

            rateLimitInfo.RequestTimes = rateLimitInfo.RequestTimes
                .Where(t => currentTime - t <= TimeWindow)
                .ToList();

            if (rateLimitInfo.RequestTimes.Count >= _maxRequestsPerMinute)
            {
                return true;  // Rate limit exceeded
            }

            rateLimitInfo.RequestTimes.Add(currentTime);
            return false;
        }



        private class RateLimitInfo
        {
            public List<DateTime> RequestTimes { get; set; } = new List<DateTime>();
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (ipAddress == null)
            {
                context.Result = new BadRequestObjectResult("Unable to determine client IP.");
                return;
            }

            if (IsRateLimited(ipAddress))
            {
                context.Result = new ObjectResult(await Response.FailureAsync($"you can't run code {_maxRequestsPerMinute} times in a minute"));
                return;
            }

            await next();
        }
    }

}
