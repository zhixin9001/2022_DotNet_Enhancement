using _4_ASP_NET_CORE_ADVANCED;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IdDbContext>(opt =>
{
    string connStr = builder.Configuration.GetConnectionString("Default");
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
    opt.UseMySql(connStr, serverVersion);
});
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
});

var idBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
idBuilder.AddEntityFrameworkStores<IdDbContext>()
    .AddDefaultTokenProviders()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<UserManager<User>>();
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