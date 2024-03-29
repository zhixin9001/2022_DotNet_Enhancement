using FileService.Domain;
using FileService.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FSDbContext>(opt =>
{
    string connStr = builder.Configuration.GetConnectionString("Default");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
    opt.UseMySql(connStr, serverVersion);
});

builder.Services
    .Configure<SMBStorageOptions>(builder.Configuration.GetSection("SMBStorage"))
    .Configure<MockCloudStorageOptions>(builder.Configuration.GetSection("MockCloudStorage"));

builder.Services.AddScoped<IFSRepository, FSRepository>();
builder.Services.AddScoped<IStorageClient, SMBStorageClient>();
builder.Services.AddScoped<IStorageClient, MockCloudStorageClient>();
builder.Services.AddScoped<FSDomainService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();