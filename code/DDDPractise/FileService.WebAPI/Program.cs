using FileService.Domain;
using FileService.Infrastructure;
using FileService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FSDbContext>(opt =>
{
    string connStr = builder.Configuration.GetConnectionString("Default");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
    opt.UseMySql(connStr, serverVersion);
});
// Add services to the container.
builder.Services.AddScoped<IStorageClient, SMBStorageClient>();
builder.Services.AddScoped<IStorageClient, MockCloudStorageClient>();
builder.Services.AddScoped<IFSRepository, FSRepository>();
builder.Services.AddScoped<FSDomainService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FileService.WebAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileService.WebAPI v1"));
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();