using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _2_EFCore_Basic;

public class TestDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Article> Article { get; set; }
    public DbSet<Comment> Comment { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=localhost;user=root;password=123456;database=efcore";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
        optionsBuilder.UseMySql(connectionString,serverVersion)
            .LogTo(Console.WriteLine,LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        
        // base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}