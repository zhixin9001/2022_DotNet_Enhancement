using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _4_ASP_NET_CORE_ADVANCED;

public class IdDbContext : IdentityDbContext<User, Role, long>
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