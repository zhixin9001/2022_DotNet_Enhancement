using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _3_ASP_NET_CORE_BASIC.Filters;

public class MyExceptionFilter: IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        context.ExceptionHandled = true;
        context.Result = new ObjectResult(new { code=500,message="exception is coming"});
        return Task.CompletedTask;
    }
}