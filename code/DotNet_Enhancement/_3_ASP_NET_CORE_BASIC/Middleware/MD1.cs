namespace _3_ASP_NET_CORE_BASIC.Middleware;

public class MD1
{
    private readonly RequestDelegate _next;

    public MD1(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await context.Response.WriteAsync("1+");
        await _next(context);
        await context.Response.WriteAsync("1-");
    }
}