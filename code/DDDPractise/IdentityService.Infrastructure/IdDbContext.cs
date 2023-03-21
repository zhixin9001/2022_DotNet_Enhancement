namespace IdentityService.Infrastructure;

public class IdDbContext : IdentityDbContext<User, Role, Guid>
{
    public IdDbContext(DbContextOptions<IdDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}