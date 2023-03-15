using FileService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ZhiXin.Infrastructure.EFCore;

namespace FileService.Infrastructure;

public class FSDbContext : BaseDbContext
{
    public DbSet<UploadedItem> UploadedItems { get; private set; }

    public FSDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}