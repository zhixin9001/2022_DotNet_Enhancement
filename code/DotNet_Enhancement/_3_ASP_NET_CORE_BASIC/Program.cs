using _3_ASP_NET_CORE_BASIC;
using _3_ASP_NET_CORE_BASIC.Filters;
using _3_ASP_NET_CORE_BASIC.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// builder.Services.AddResponseCaching();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add<RateLimitFilter>();
    // options.Filters.Add<MyExceptionFilter>();
});
builder.Services.AddMemoryCache();
var app = builder.Build();

app.Map("/test", async appBuilder =>
{
    // appBuilder.Use(async (context, next) =>
    // {
    //     // context.Response.ContentType = "text/html";
    //     await context.Response.WriteAsync("1+");
    //     await next();
    //     await context.Response.WriteAsync("1-");
    // });
    // appBuilder.Use(async (context, next) =>
    // {
    //     await context.Response.WriteAsync("2+");
    //     await next();
    //     await context.Response.WriteAsync("2-");
    // });
    appBuilder.UseMiddleware<MD2>();
    appBuilder.UseMiddleware<MD1>();
    appBuilder.Run(async ctx =>
    {
        await ctx.Response.WriteAsync("[]");
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();

app.Run();