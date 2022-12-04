namespace _3_ASP_NET_CORE_BASIC.Middleware;

public class MD2
{
    private readonly RequestDelegate _next;

    public MD2(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await context.Response.WriteAsync("2+");
        await _next(context);
        await context.Response.WriteAsync("2-");
    }
}