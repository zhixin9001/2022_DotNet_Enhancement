using Microsoft.EntityFrameworkCore.Design;

namespace IdDomainService.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdDbContext>
{
    public IdDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<IdDbContext> builder = new();
        // var connStr = "server=localhost;user=root;password=123456;database=DDDPractise";
        var connStr = Environment.GetEnvironmentVariable("ConnectionStrings")!;
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        builder.UseMySql(connStr, serverVersion);
        return new IdDbContext(builder.Options);
    }
}