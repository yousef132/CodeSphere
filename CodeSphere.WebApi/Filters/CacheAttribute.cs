namespace CodeSphere.WebApi.Filters
{
    //public class CacheAttribute : Attribute, IAsyncActionFilter
    //{
    //    private readonly int timeToLive;

    //    public CacheAttribute(int timeToLive)
    //    {
    //        this.timeToLive = timeToLive;
    //    }
    //    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        // ask clr for creating object from ResponseCacheService explicitly
    //        var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
    //        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
    //        var response = await responseCacheService.GetCachedResponseAsync<IEnumerable<Problem>>(cacheKey);

    //        if (!string.IsNullOrEmpty(response))
    //        {
    //            // response is cached

    //            var result = new ContentResult
    //            {
    //                Content = response,
    //                ContentType = "application/json",
    //                StatusCode = 200,
    //            };
    //            context.Result = result;
    //            return;
    //        }
    //        // invoke the action when the response is not cached
    //        var executedActionContext = await next.Invoke();

    //        if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
    //        {
    //            await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLive));
    //        }

    //    }

    //    private string GenerateCacheKeyFromRequest(HttpRequest request)
    //    {
    //        // key : unique for each request so generate it from request
    //        // generate it from URL Path + Query String 

    //        StringBuilder keyBuilder = new StringBuilder();

    //        keyBuilder.Append(request.Path);// api/product

    //        //pageIndex=2&
    //        //pageSize=6&
    //        //sort=name

    //        // Key : api/product|pageIndex-2|pageSize-6|sort-name
    //        // Ordered by key to handle cases when the order of query string parameters changes but the values remain the same.

    //        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
    //        {
    //            keyBuilder.Append($"|{key}-{value}");
    //        }

    //        return keyBuilder.ToString();
    //    }
    //}


}
