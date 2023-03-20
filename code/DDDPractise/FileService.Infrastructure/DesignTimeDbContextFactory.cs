using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FileService.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FSDbContext>
{
    public FSDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<FSDbContext> builder = new();
        var connStr = "server=localhost;user=root;password=123456;database=DDDPractise";
        // var connStr = Environment.GetEnvironmentVariable("ConnectionStrings:")!;
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        builder.UseMySql(connStr, serverVersion);
        return new FSDbContext(builder.Options);
    }
}