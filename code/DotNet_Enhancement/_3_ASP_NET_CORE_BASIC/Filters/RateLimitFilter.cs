using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace _3_ASP_NET_CORE_BASIC.Filters;

public class RateLimitFilter: IAsyncActionFilter
{
    private readonly IMemoryCache _memoryCache;

    public RateLimitFilter(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        string remoteIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
        var lastTick = _memoryCache.Get<long?>(remoteIp);
        if (lastTick == null || Environment.TickCount64 - lastTick > 1000)
        {
            _memoryCache.Set(remoteIp, Environment.TickCount64, TimeSpan.FromSeconds(2));
            return next();
        }
        context.Result = new ContentResult() {StatusCode = 429, Content = "test rate limit"};
        return Task.CompletedTask;
    }
}